namespace DataBaseEditor.Tables;

partial class BaseHeroesForm
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
        textBox_name = new TextBox();
        label1 = new Label();
        numeric_rarity = new NumericUpDown();
        label2 = new Label();
        ucDice_Health = new UCDice();
        button_save = new Button();
        ucDice_Damage = new UCDice();
        ((System.ComponentModel.ISupportInitialize)numeric_rarity).BeginInit();
        SuspendLayout();
        // 
        // textBox_name
        // 
        textBox_name.Location = new Point(120, 13);
        textBox_name.Margin = new Padding(4);
        textBox_name.Name = "textBox_name";
        textBox_name.Size = new Size(127, 29);
        textBox_name.TabIndex = 0;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(63, 16);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(49, 21);
        label1.TabIndex = 1;
        label1.Text = "name";
        // 
        // numeric_rarity
        // 
        numeric_rarity.Location = new Point(120, 49);
        numeric_rarity.Name = "numeric_rarity";
        numeric_rarity.Size = new Size(127, 29);
        numeric_rarity.TabIndex = 2;
        numeric_rarity.TextAlign = HorizontalAlignment.Right;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(64, 51);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(47, 21);
        label2.TabIndex = 1;
        label2.Text = "rarity";
        // 
        // ucDice_Health
        // 
        ucDice_Health.BackColor = Color.FromArgb(192, 255, 255);
        ucDice_Health.Font = new Font("Segoe UI", 12F);
        ucDice_Health.Location = new Point(3, 85);
        ucDice_Health.Margin = new Padding(4);
        ucDice_Health.MaximumSize = new Size(358, 141);
        ucDice_Health.MinimumSize = new Size(358, 141);
        ucDice_Health.Name = "ucDice_Health";
        ucDice_Health.Padding = new Padding(5);
        ucDice_Health.Size = new Size(358, 141);
        ucDice_Health.TabIndex = 3;
        // 
        // button_save
        // 
        button_save.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        button_save.Location = new Point(426, 444);
        button_save.Name = "button_save";
        button_save.Size = new Size(114, 37);
        button_save.TabIndex = 4;
        button_save.Text = "Save";
        button_save.UseVisualStyleBackColor = true;
        button_save.Click += button_save_Click;
        // 
        // ucDice_Damage
        // 
        ucDice_Damage.BackColor = Color.FromArgb(192, 255, 255);
        ucDice_Damage.Font = new Font("Segoe UI", 12F);
        ucDice_Damage.Location = new Point(3, 234);
        ucDice_Damage.Margin = new Padding(4);
        ucDice_Damage.MaximumSize = new Size(358, 141);
        ucDice_Damage.MinimumSize = new Size(358, 141);
        ucDice_Damage.Name = "ucDice_Damage";
        ucDice_Damage.Padding = new Padding(5);
        ucDice_Damage.Size = new Size(358, 141);
        ucDice_Damage.TabIndex = 3;
        // 
        // BaseHeroesForm
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Silver;
        ClientSize = new Size(543, 484);
        Controls.Add(button_save);
        Controls.Add(ucDice_Damage);
        Controls.Add(ucDice_Health);
        Controls.Add(numeric_rarity);
        Controls.Add(textBox_name);
        Controls.Add(label1);
        Controls.Add(label2);
        Font = new Font("Segoe UI", 12F);
        Margin = new Padding(4);
        Name = "BaseHeroesForm";
        Text = "BaseHeroesForm";
        FormClosing += BaseHeroesForm_FormClosing;
        Load += BaseHeroesForm_Load;
        ((System.ComponentModel.ISupportInitialize)numeric_rarity).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox textBox_name;
    private Label label1;
    private NumericUpDown numeric_rarity;
    private Label label2;
    private UCDice ucDice_Health;
    private Button button_save;
    private UCDice ucDice_Damage;
}
