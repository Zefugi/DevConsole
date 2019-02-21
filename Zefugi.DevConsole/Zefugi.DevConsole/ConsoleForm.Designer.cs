namespace Zefugi.DevConsole
{
    partial class ConsoleForm
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
            this.components = new System.ComponentModel.Container();
            this._txtInput = new System.Windows.Forms.TextBox();
            this._txtLog = new System.Windows.Forms.RichTextBox();
            this._logTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _txtInput
            // 
            this._txtInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this._txtInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._txtInput.Font = new System.Drawing.Font("Courier New", 9.75F);
            this._txtInput.ForeColor = System.Drawing.Color.White;
            this._txtInput.Location = new System.Drawing.Point(0, 593);
            this._txtInput.Name = "_txtInput";
            this._txtInput.Size = new System.Drawing.Size(852, 15);
            this._txtInput.TabIndex = 0;
            this._txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this._txtInput_KeyDown);
            this._txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._txtInput_KeyPress);
            // 
            // _txtLog
            // 
            this._txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this._txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtLog.ForeColor = System.Drawing.Color.White;
            this._txtLog.Location = new System.Drawing.Point(0, 0);
            this._txtLog.Name = "_txtLog";
            this._txtLog.ReadOnly = true;
            this._txtLog.Size = new System.Drawing.Size(852, 593);
            this._txtLog.TabIndex = 1;
            this._txtLog.Text = "";
            this._txtLog.WordWrap = false;
            // 
            // _logTimer
            // 
            this._logTimer.Enabled = true;
            this._logTimer.Interval = 10;
            this._logTimer.Tick += new System.EventHandler(this._logTimer_Tick);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(852, 608);
            this.Controls.Add(this._txtLog);
            this.Controls.Add(this._txtInput);
            this.Name = "ConsoleForm";
            this.Text = "Zefugi - DevConsole";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtInput;
        private System.Windows.Forms.RichTextBox _txtLog;
        private System.Windows.Forms.Timer _logTimer;
    }
}

