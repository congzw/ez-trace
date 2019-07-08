namespace EzTrace.DemoApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtOps = new System.Windows.Forms.TextBox();
            this.btnCall = new System.Windows.Forms.Button();
            this.txtLogs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxCount = new System.Windows.Forms.ComboBox();
            this.cbxSeconds = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCall2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "MockNestedOps";
            // 
            // txtOps
            // 
            this.txtOps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOps.Location = new System.Drawing.Point(107, 190);
            this.txtOps.Name = "txtOps";
            this.txtOps.Size = new System.Drawing.Size(382, 21);
            this.txtOps.TabIndex = 7;
            // 
            // btnCall
            // 
            this.btnCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCall.Location = new System.Drawing.Point(497, 188);
            this.btnCall.Name = "btnCall";
            this.btnCall.Size = new System.Drawing.Size(75, 23);
            this.btnCall.TabIndex = 6;
            this.btnCall.Text = "Call";
            this.btnCall.UseVisualStyleBackColor = true;
            this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
            // 
            // txtLogs
            // 
            this.txtLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogs.Location = new System.Drawing.Point(12, 12);
            this.txtLogs.Multiline = true;
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.Size = new System.Drawing.Size(560, 147);
            this.txtLogs.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "CallTimes";
            // 
            // cbxCount
            // 
            this.cbxCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCount.FormattingEnabled = true;
            this.cbxCount.Location = new System.Drawing.Point(107, 165);
            this.cbxCount.Name = "cbxCount";
            this.cbxCount.Size = new System.Drawing.Size(68, 20);
            this.cbxCount.TabIndex = 12;
            // 
            // cbxSeconds
            // 
            this.cbxSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSeconds.FormattingEnabled = true;
            this.cbxSeconds.Location = new System.Drawing.Point(293, 164);
            this.cbxSeconds.Name = "cbxSeconds";
            this.cbxSeconds.Size = new System.Drawing.Size(68, 20);
            this.cbxSeconds.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(192, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "CallEverySecond";
            // 
            // btnCall2
            // 
            this.btnCall2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCall2.Location = new System.Drawing.Point(497, 162);
            this.btnCall2.Name = "btnCall2";
            this.btnCall2.Size = new System.Drawing.Size(75, 23);
            this.btnCall2.TabIndex = 15;
            this.btnCall2.Text = "Call2";
            this.btnCall2.UseVisualStyleBackColor = true;
            this.btnCall2.Click += new System.EventHandler(this.btnCall2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 223);
            this.Controls.Add(this.btnCall2);
            this.Controls.Add(this.cbxSeconds);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLogs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOps);
            this.Controls.Add(this.btnCall);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOps;
        private System.Windows.Forms.Button btnCall;
        private System.Windows.Forms.TextBox txtLogs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxCount;
        private System.Windows.Forms.ComboBox cbxSeconds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCall2;
    }
}