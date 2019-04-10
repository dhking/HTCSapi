namespace SignApplication
{
    partial class user
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
            this.label2 = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.user_code = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.id_card_no = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mobile = new System.Windows.Forms.TextBox();
            this.tas = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "name：";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(143, 123);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(183, 21);
            this.name.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "user_code：";
            // 
            // user_code
            // 
            this.user_code.Location = new System.Drawing.Point(143, 96);
            this.user_code.Name = "user_code";
            this.user_code.Size = new System.Drawing.Size(183, 21);
            this.user_code.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "id_card_no：";
            // 
            // id_card_no
            // 
            this.id_card_no.Location = new System.Drawing.Point(143, 161);
            this.id_card_no.Name = "id_card_no";
            this.id_card_no.Size = new System.Drawing.Size(183, 21);
            this.id_card_no.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(78, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "mobile：";
            // 
            // mobile
            // 
            this.mobile.Location = new System.Drawing.Point(143, 199);
            this.mobile.Name = "mobile";
            this.mobile.Size = new System.Drawing.Size(183, 21);
            this.mobile.TabIndex = 1;
            // 
            // tas
            // 
            this.tas.Location = new System.Drawing.Point(240, 256);
            this.tas.Name = "tas";
            this.tas.Size = new System.Drawing.Size(75, 23);
            this.tas.TabIndex = 2;
            this.tas.Text = "申请证书";
            this.tas.UseVisualStyleBackColor = true;
            this.tas.Click += new System.EventHandler(this.tas_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 256);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "返回";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // user
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 358);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tas);
            this.Controls.Add(this.mobile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.id_card_no);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.user_code);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.name);
            this.Controls.Add(this.label2);
            this.Name = "user";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "申请证书";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox user_code;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox id_card_no;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mobile;
        private System.Windows.Forms.Button tas;
        private System.Windows.Forms.Button button1;
    }
}