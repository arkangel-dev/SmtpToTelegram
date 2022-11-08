using SmtpForwarder;

namespace UI {
    public partial class Home : Form {
        public Home() {
            InitializeComponent();
        }

        Forwarder? fw;
        FixedConfigReader configReader;

        private void Home_Load(object sender, EventArgs e) {
            this.fw = new Forwarder();
            this.configReader = fw.GetConfigReader();

            Hostname_UIControl.DataBindings.Add("Text", configReader.GetConfigValue("hostname"), "StringValue", true, DataSourceUpdateMode.OnValidation);
            PortNumber_UIControl.DataBindings.Add("Value", configReader.GetConfigValue("port"), "IntegerValue", true, DataSourceUpdateMode.OnValidation);
            BotToken_UIControl.DataBindings.Add("Text", configReader.GetConfigValue("bot_token"), "StringValue", true, DataSourceUpdateMode.OnValidation);
            BotTarget_UIControl.DataBindings.Add("Text", configReader.GetConfigValue("target_chat"), "StringValue", true, DataSourceUpdateMode.OnValidation);

            fw.OnDebug += Fw_OnDebug;

            try {
                fw.Start();
            } catch (Exception ex) {
                Fw_OnDebug(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void Fw_OnDebug(string s) {
            SetText(s + "\n");
        }

        private void RestartServerButton_Click(object sender, EventArgs e) {
            Fw_OnDebug("-- RESTARTING --\n\n");
            fw?.Restart();
        }

     

        delegate void SetTextCallback(string text);

        private void SetText(string text) {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.DebugWindow_UIControl.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            } else {
                this.DebugWindow_UIControl.Text += text;
            }
        }
    }
}