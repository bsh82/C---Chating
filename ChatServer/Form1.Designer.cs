namespace ChatServer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtChatMsg = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtChatMsg
            // 
            this.txtChatMsg.Location = new System.Drawing.Point(31, 35);
            this.txtChatMsg.Multiline = true;
            this.txtChatMsg.Name = "txtChatMsg";
            this.txtChatMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatMsg.Size = new System.Drawing.Size(506, 377);
            this.txtChatMsg.TabIndex = 0;
            this.txtChatMsg.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnStart.Location = new System.Drawing.Point(304, 424);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(233, 59);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "서버 시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(56, 448);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(94, 20);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Tag = "Stop";
            this.lblMsg.Text = "서버 중지 됨";
            this.lblMsg.Click += new System.EventHandler(this.lblMsg_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 511);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtChatMsg);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtChatMsg;
        private Button btnStart;
        private Label lblMsg;
    }
}