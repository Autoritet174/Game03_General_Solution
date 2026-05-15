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
        ucDiceHealth = new UCDice();
        button1 = new Button();
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
        // ucDiceHealth
        // 
        ucDiceHealth.Font = new Font("Segoe UI", 12F);
        ucDiceHealth.Location = new Point(3, 85);
        ucDiceHealth.Margin = new Padding(4);
        ucDiceHealth.Name = "ucDiceHealth";
        ucDiceHealth.Size = new Size(285, 121);
        ucDiceHealth.TabIndex = 3;
        // 
        // button1
        // 
        button1.Location = new Point(251, 301);
        button1.Name = "button1";
        button1.Size = new Size(108, 28);
        button1.TabIndex = 4;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // BaseHeroesForm
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(765, 450);
        Controls.Add(button1);
        Controls.Add(ucDiceHealth);
        Controls.Add(numeric_rarity);
        Controls.Add(textBox_name);
        Controls.Add(label1);
        Controls.Add(label2);
        Font = new Font("Segoe UI", 12F);
        Margin = new Padding(4);
        Name = "BaseHeroesForm";
        Text = "BaseHeroesForm";
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
    private UCDice ucDiceHealth;
    private Button button1;
}