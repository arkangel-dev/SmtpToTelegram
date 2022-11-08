using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmtpForwarder {
    public class FixedConfigReader {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DataSource">Datasource</param>
        public FixedConfigReader(JObject DataSource) {
            this.DataSource = DataSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DataSource">Datasource</param>
        /// <param name="segments">Configuration segments to add to this reader</param>
        public FixedConfigReader(JObject DataSource, Action<FixedConfigReader>? OnChangeAction, params FixedConfigReaderSegment[] segments) {
            this.DataSource = DataSource;
            this.OnValueChange = OnChangeAction;
            foreach (var segment in segments) {
                segment.ParentReader = this;
            }
            this.Segments.AddRange(segments);
        }

        /// <summary>
        /// Add a segment and register this instance of the reader as its parent
        /// </summary>
        /// <param name="segment">Segment to add</param>
        public void AddSegment(FixedConfigReaderSegment segment) {
            segment.ParentReader = this;
            this.Segments.Add(segment);
        }

        /// <summary>
        /// Add mutliple segments and register this instance of the reader as its parent
        /// </summary>
        /// <param name="segments"></param>
        public void AddSegments(IEnumerable<FixedConfigReaderSegment> segments) {
            foreach (var segment in segments) {
                AddSegment(segment);
            }
        }

        /// <summary>
        /// Delegate for when a value gets updated
        /// </summary>
        /// <param name="r">Source config reader</param>
        /// <param name="s">Source config segment</param>
        /// <param name="v">New value</param>
        public delegate void OnUpdateDelegate(FixedConfigReader r, FixedConfigReaderSegment s, object v);

        /// <summary>
        /// Event that gets triggered when a value gets updated
        /// </summary>
        public event OnUpdateDelegate OnUpdate;



        /// <summary>
        /// Finds a segment that matches path provided as parameter and returns its value accessor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="InvalidParameterException"></exception>
        public FixedConfigReaderSegment GetConfigValue(params string[] path) {
            foreach (var c in this.Segments) {
                if (c.ConfigPath.SequenceEqual(path)) {

                    return c;
                }
            }
            throw new InvalidParameterException("Cannot find data on path : " + String.Join('/', path));
        }

        private object[] Reference;

        /// <summary>
        /// Configuration segments assigned to this reader
        /// </summary>
        public List<FixedConfigReaderSegment> Segments = new List<FixedConfigReaderSegment>();

        /// <summary>
        ///  Datasource to use for this reader
        /// </summary>
        public JObject DataSource;

        /// <summary>
        /// Action to be taken once the value has been changed. Useful for stuff like
        /// saving the datasource to the persistence layer if applicable
        /// </summary>
        public Action<FixedConfigReader>? OnValueChange { get; set; }




        /// <summary>
        /// A configuration segment represents a configuration that can read from the datasource.
        /// Each segment will store a path, and a default value that is a primitive supported by
        /// Newtonsoft JSON.NET. The values are read and written as need to memory from the 
        /// datasource
        /// </summary>
        public class FixedConfigReaderSegment {
            /// <summary>
            /// Path of the configuration
            /// </summary>
            internal string[] ConfigPath { get; set; }

            public bool BooleanValue {
                get {
                    return Value.Value<bool?>() ?? throw new Exception("Failed to convert token");
                }
                set {
                    this.Value = JToken.FromObject(value);
                }
            }

            public string StringValue {
                get {
                    return Value.Value<string?>() ?? throw new Exception("Failed to convert token");
                }
                set {
                    this.Value = JToken.FromObject(value);
                }
            }

            public int IntegerValue {
                get {
                    return Value.Value<int?>() ?? throw new Exception("Failed to convert token"); 
                }
                set {
                    this.Value = JToken.FromObject(value);
                }
            }

            public Func<JToken, bool> ValidationCheck = (JToken o) => true;

            /// <summary>
            /// Value from the data source
            /// </summary>
            public JToken Value {
                get {
                    // if the current value is null then try to read the value from the datasource.
                    // if the datasource has no value then return the pre-defined default value
                    if (_value == null) if (!ReadValueFromSource()) return _defaultValue;
                    return _value;
                }
                set {
                    if (_value == value) return;
                    if (!ValidationCheck.Invoke(value)) return;
                    _value = value;
                    SetValueToSource();
                    ParentReader.OnUpdate?.Invoke(this.ParentReader, this, value);
                    ParentReader.OnValueChange?.Invoke(this.ParentReader);

                }
            }

            /// <summary>
            /// Set the current non-null value to the datasource
            /// </summary>
            /// <returns></returns>
            private bool SetValueToSource() {
                if (this._value == null) return false;
                return SetJObjectValue(this.ParentReader.DataSource, this._value, this.ConfigPath);
            }

            /// <summary>
            /// Try to read the value from the datasource.
            /// </summary>
            /// <returns>Flag indicating whether the read result was null or not</returns>
            private bool ReadValueFromSource() {
                this._value = GetJObjectValue<JToken>(ParentReader.DataSource, ConfigPath);
                return this._value != null;
            }
            public object target = 0;



            public static T? GetJObjectValue<T>(JObject jobj, params string[] path) {
                try {
                    JObject? currentLayer;
                    if (path.Count() > 1) {
                        currentLayer = jobj.Value<JObject>(path[0]);
                    } else {
                        return jobj.Value<T>(path[0]);
                    }
                    for (int i = 1; i < path.Count(); i++) {
                        if (currentLayer == null) return default(T);
                        if (i != path.Count() - 1) {
                            currentLayer = currentLayer.Value<JObject>(path[i]);
                        } else {
                            var result = currentLayer.Value<T>(path[i]);
                            return result;
                        }
                    }
                    return default(T);
                } catch (Exception e) {
                    return default(T);
                }
            }

            /// <summary>
            /// Set the value of a jobject from a list of keys. If a key is missing from a path array, new keys will be added.
            /// </summary>
            /// <param name="jobj">JObject to perform the operation on</param>
            /// <param name="value">Value to set</param>
            /// <param name="path">Path to value</param>
            /// <returns>Flag indicating whether or not the write operation was successful</returns>
            /// <exception cref="DataException"></exception>
            public static bool SetJObjectValue(JObject jobj, JToken value, params string[] path) {
                JObject CurrentLayer = jobj;
                JObject RootNode = CurrentLayer;
                for (int i = 0; i < path.Length; i++) {
                    var key = path[i];
                    if (!CurrentLayer.ContainsKey(key)) {

                        if (i == path.Length - 1) {
                            CurrentLayer.Add(new JProperty(key, value));
                            return true;
                        } else {
                            var NewObj = new JObject();
                            CurrentLayer.Add(new JProperty(key, NewObj));
                            CurrentLayer = NewObj;
                        }
                    } else {
                        if (i == path.Length - 1) {
                            CurrentLayer.Property(key).Value = value;
                        } else {
                            CurrentLayer = CurrentLayer.Value<JObject>(key) ?? throw new DataException("Failed to parse");
                        }
                    }
                }
                return false;
            }

            // value holder and default value holder
            private JToken? _value;
            private JToken _defaultValue;

            // Parent of this configuration segment
            internal FixedConfigReader? ParentReader;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="defaultValue">Default value</param>
            /// <param name="path">Path to configuration</param>
            public FixedConfigReaderSegment(JToken defaultValue, params string[] path) {
                this.ConfigPath = path;
                this._defaultValue = defaultValue;
            }
        }
    }
}
