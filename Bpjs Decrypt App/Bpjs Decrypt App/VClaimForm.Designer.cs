namespace Bpjs_Decrypt_App
{
    partial class VClaimForm
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
            richTextBox1 = new RichTextBox();
            label9 = new Label();
            tbURL = new TextBox();
            btnSend = new Button();
            label5 = new Label();
            tbConsId = new TextBox();
            label6 = new Label();
            tbSecretKey = new TextBox();
            label7 = new Label();
            tbUserKey = new TextBox();
            label2 = new Label();
            tbService = new TextBox();
            label1 = new Label();
            tbBaseUrl = new TextBox();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.Location = new Point(635, 10);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(574, 258);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(18, 98);
            label9.Name = "label9";
            label9.Size = new Size(28, 15);
            label9.TabIndex = 18;
            label9.Text = "URL";
            // 
            // tbURL
            // 
            tbURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbURL.Location = new Point(102, 95);
            tbURL.Name = "tbURL";
            tbURL.Size = new Size(500, 23);
            tbURL.TabIndex = 17;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.DodgerBlue;
            btnSend.FlatStyle = FlatStyle.Popup;
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(102, 208);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(129, 33);
            btnSend.TabIndex = 16;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 183);
            label5.Name = "label5";
            label5.Size = new Size(47, 15);
            label5.TabIndex = 15;
            label5.Text = "Cons Id";
            // 
            // tbConsId
            // 
            tbConsId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbConsId.Location = new Point(102, 180);
            tbConsId.Name = "tbConsId";
            tbConsId.Size = new Size(500, 23);
            tbConsId.TabIndex = 14;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 155);
            label6.Name = "label6";
            label6.Size = new Size(61, 15);
            label6.TabIndex = 13;
            label6.Text = "Secret Key";
            // 
            // tbSecretKey
            // 
            tbSecretKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbSecretKey.Location = new Point(102, 152);
            tbSecretKey.Name = "tbSecretKey";
            tbSecretKey.Size = new Size(500, 23);
            tbSecretKey.TabIndex = 12;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 127);
            label7.Name = "label7";
            label7.Size = new Size(52, 15);
            label7.TabIndex = 11;
            label7.Text = "User Key";
            // 
            // tbUserKey
            // 
            tbUserKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbUserKey.Location = new Point(102, 124);
            tbUserKey.Name = "tbUserKey";
            tbUserKey.Size = new Size(500, 23);
            tbUserKey.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 70);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 3;
            label2.Text = "Service";
            // 
            // tbService
            // 
            tbService.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbService.Location = new Point(102, 67);
            tbService.Name = "tbService";
            tbService.Size = new Size(500, 23);
            tbService.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 42);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 1;
            label1.Text = "Base URL";
            // 
            // tbBaseUrl
            // 
            tbBaseUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbBaseUrl.Location = new Point(102, 39);
            tbBaseUrl.Name = "tbBaseUrl";
            tbBaseUrl.Size = new Size(500, 23);
            tbBaseUrl.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(tbURL);
            groupBox1.Controls.Add(btnSend);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(tbConsId);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(tbSecretKey);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(tbUserKey);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(tbService);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(tbBaseUrl);
            groupBox1.Location = new Point(13, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(616, 258);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "VClaim Credentials";
            // 
            // VClaimForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1222, 278);
            Controls.Add(richTextBox1);
            Controls.Add(groupBox1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "VClaimForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "VClaimForm";
            Load += VClaimForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Label label9;
        private TextBox tbURL;
        private Button btnSend;
        private Label label5;
        private TextBox tbConsId;
        private Label label6;
        private TextBox tbSecretKey;
        private Label label7;
        private TextBox tbUserKey;
        private Label label2;
        private TextBox tbService;
        private Label label1;
        private TextBox tbBaseUrl;
        private GroupBox groupBox1;
    }
}