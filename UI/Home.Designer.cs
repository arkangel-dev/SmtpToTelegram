namespace UI {
    partial class Home {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.RestartServerButton = new System.Windows.Forms.Button();
            this.DebugWindow_UIControl = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.PortNumber_UIControl = new System.Windows.Forms.NumericUpDown();
            this.BotTarget_UIControl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BotToken_UIControl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Hostname_UIControl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortNumber_UIControl)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.RestartServerButton);
            this.tabPage1.Controls.Add(this.DebugWindow_UIControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Debug";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // RestartServerButton
            // 
            this.RestartServerButton.Location = new System.Drawing.Point(603, 358);
            this.RestartServerButton.Name = "RestartServerButton";
            this.RestartServerButton.Size = new System.Drawing.Size(159, 29);
            this.RestartServerButton.TabIndex = 1;
            this.RestartServerButton.Text = "Restart Server";
            this.RestartServerButton.UseVisualStyleBackColor = true;
            this.RestartServerButton.Click += new System.EventHandler(this.RestartServerButton_Click);
            // 
            // DebugWindow_UIControl
            // 
            this.DebugWindow_UIControl.Location = new System.Drawing.Point(6, 6);
            this.DebugWindow_UIControl.Name = "DebugWindow_UIControl";
            this.DebugWindow_UIControl.Size = new System.Drawing.Size(756, 346);
            this.DebugWindow_UIControl.TabIndex = 0;
            this.DebugWindow_UIControl.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.PortNumber_UIControl);
            this.tabPage2.Controls.Add(this.BotTarget_UIControl);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.BotToken_UIControl);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.Hostname_UIControl);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Config";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // PortNumber_UIControl
            // 
            this.PortNumber_UIControl.Location = new System.Drawing.Point(127, 63);
            this.PortNumber_UIControl.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.PortNumber_UIControl.Name = "PortNumber_UIControl";
            this.PortNumber_UIControl.Size = new System.Drawing.Size(613, 27);
            this.PortNumber_UIControl.TabIndex = 8;
            // 
            // BotTarget_UIControl
            // 
            this.BotTarget_UIControl.Location = new System.Drawing.Point(127, 128);
            this.BotTarget_UIControl.Name = "BotTarget_UIControl";
            this.BotTarget_UIControl.Size = new System.Drawing.Size(613, 27);
            this.BotTarget_UIControl.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Bot Target";
            // 
            // BotToken_UIControl
            // 
            this.BotToken_UIControl.Location = new System.Drawing.Point(127, 95);
            this.BotToken_UIControl.Name = "BotToken_UIControl";
            this.BotToken_UIControl.Size = new System.Drawing.Size(613, 27);
            this.BotToken_UIControl.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Bot Token";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // Hostname_UIControl
            // 
            this.Hostname_UIControl.Location = new System.Drawing.Point(127, 29);
            this.Hostname_UIControl.Name = "Hostname_UIControl";
            this.Hostname_UIControl.Size = new System.Drawing.Size(613, 27);
            this.Hostname_UIControl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hostname";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.Text = "SMTP to Telegram";
            this.Load += new System.EventHandler(this.Home_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortNumber_UIControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button RestartServerButton;
        private RichTextBox DebugWindow_UIControl;
        private TabPage tabPage2;
        private NumericUpDown PortNumber_UIControl;
        private TextBox BotTarget_UIControl;
        private Label label4;
        private TextBox BotToken_UIControl;
        private Label label3;
        private Label label2;
        private TextBox Hostname_UIControl;
        private Label label1;
    }
}