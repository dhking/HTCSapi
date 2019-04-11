namespace SignApplication
{
    partial class configWin
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.zqid = new System.Windows.Forms.TextBox();
            this.publicKey = new System.Windows.Forms.TextBox();
            this.privateKey = new System.Windows.Forms.TextBox();
            this.service_url = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "zqid：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "public_key：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "private_key：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 271);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "服务器地址：";
            // 
            // zqid
            // 
            this.zqid.Location = new System.Drawing.Point(177, 72);
            this.zqid.Name = "zqid";
            this.zqid.Size = new System.Drawing.Size(295, 21);
            this.zqid.TabIndex = 4;
            // 
            // publicKey
            // 
            this.publicKey.Location = new System.Drawing.Point(177, 134);
            this.publicKey.Name = "publicKey";
            this.publicKey.Size = new System.Drawing.Size(295, 21);
            this.publicKey.TabIndex = 4;
            // 
            // privateKey
            // 
            this.privateKey.Location = new System.Drawing.Point(177, 201);
            this.privateKey.Name = "privateKey";
            this.privateKey.Size = new System.Drawing.Size(295, 21);
            this.privateKey.TabIndex = 4;
            // 
            // service_url
            // 
            this.service_url.Location = new System.Drawing.Point(177, 268);
            this.service_url.Name = "service_url";
            this.service_url.Size = new System.Drawing.Size(295, 21);
            this.service_url.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(397, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "修改配置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(177, 356);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "返回";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // configWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 452);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.service_url);
            this.Controls.Add(this.privateKey);
            this.Controls.Add(this.publicKey);
            this.Controls.Add(this.zqid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "configWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数配置";
            this.Load += new System.EventHandler(this.configWin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox zqid;
        private System.Windows.Forms.TextBox publicKey;
        private System.Windows.Forms.TextBox privateKey;
        private System.Windows.Forms.TextBox service_url;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}