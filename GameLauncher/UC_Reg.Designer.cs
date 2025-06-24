namespace GameLauncher;

partial class UC_Reg
{
    /// <summary> 
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором компонентов

    /// <summary> 
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
        kryptonLabel_email = new Krypton.Toolkit.KryptonLabel();
        kryptonLabel_Logo = new Krypton.Toolkit.KryptonLabel();
        kryptonTextBox1 = new Krypton.Toolkit.KryptonTextBox();
        SuspendLayout();
        // 
        // kryptonLabel_email
        // 
        kryptonLabel_email.Location = new Point(261, 153);
        kryptonLabel_email.Name = "kryptonLabel_email";
        kryptonLabel_email.Size = new Size(115, 23);
        kryptonLabel_email.StateCommon.ShortText.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
        kryptonLabel_email.TabIndex = 0;
        kryptonLabel_email.Values.Text = "kryptonLabel1";
        // 
        // kryptonLabel_Logo
        // 
        kryptonLabel_Logo.Location = new Point(3, 3);
        kryptonLabel_Logo.Name = "kryptonLabel_Logo";
        kryptonLabel_Logo.Size = new Size(193, 43);
        kryptonLabel_Logo.StateCommon.ShortText.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, 204);
        kryptonLabel_Logo.TabIndex = 0;
        kryptonLabel_Logo.Values.Text = "Registration";
        // 
        // kryptonTextBox1
        // 
        kryptonTextBox1.Location = new Point(261, 182);
        kryptonTextBox1.Name = "kryptonTextBox1";
        kryptonTextBox1.Size = new Size(289, 44);
        kryptonTextBox1.StateCommon.Content.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, 204);
        kryptonTextBox1.TabIndex = 1;
        kryptonTextBox1.Text = "kryptonTextBox1";
        // 
        // UC_Reg
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(kryptonTextBox1);
        Controls.Add(kryptonLabel_Logo);
        Controls.Add(kryptonLabel_email);
        Name = "UC_Reg";
        Size = new Size(755, 433);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Krypton.Toolkit.KryptonLabel kryptonLabel_email;
    private Krypton.Toolkit.KryptonLabel kryptonLabel_Logo;
    private Krypton.Toolkit.KryptonTextBox kryptonTextBox1;
}
