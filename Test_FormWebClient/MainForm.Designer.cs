
    partial class MainForm {
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
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        textBox1 = new TextBox();
        timer1 = new System.Windows.Forms.Timer(components);
        textBox_email = new TextBox();
        textBox_password = new TextBox();
        label1 = new Label();
        label2 = new Label();
        button1 = new Button();
        button2 = new Button();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new Point(552, 12);
        textBox1.Multiline = true;
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(236, 339);
        textBox1.TabIndex = 0;
        // 
        // timer1
        // 
        timer1.Tick += timer1_Tick;
        // 
        // textBox_email
        // 
        textBox_email.Location = new Point(139, 74);
        textBox_email.Name = "textBox_email";
        textBox_email.Size = new Size(212, 23);
        textBox_email.TabIndex = 1;
        textBox_email.Text = "Admin@Admin.ru";
        // 
        // textBox_password
        // 
        textBox_password.Location = new Point(139, 129);
        textBox_password.Name = "textBox_password";
        textBox_password.Size = new Size(212, 23);
        textBox_password.TabIndex = 1;
        textBox_password.Text = "123qwe";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(139, 111);
        label1.Name = "label1";
        label1.Size = new Size(57, 15);
        label1.TabIndex = 2;
        label1.Text = "password";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(139, 56);
        label2.Name = "label2";
        label2.Size = new Size(36, 15);
        label2.TabIndex = 2;
        label2.Text = "email";
        // 
        // button1
        // 
        button1.Location = new Point(139, 158);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 3;
        button1.Text = "reg";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // button2
        // 
        button2.Location = new Point(220, 158);
        button2.Name = "button2";
        button2.Size = new Size(75, 23);
        button2.TabIndex = 3;
        button2.Text = "auth";
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(button2);
        Controls.Add(button1);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(textBox_password);
        Controls.Add(textBox_email);
        Controls.Add(textBox1);
        Name = "MainForm";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox textBox1;
    private System.Windows.Forms.Timer timer1;
    private TextBox textBox_email;
    private TextBox textBox_password;
    private Label label1;
    private Label label2;
    private Button button1;
    private Button button2;
}
