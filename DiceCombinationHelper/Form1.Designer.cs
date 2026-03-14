namespace DiceCombinationHelper;

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
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        numeric_value = new NumericUpDown();
        textBox_Results = new TextBox();
        numeric_mod = new NumericUpDown();
        label1 = new Label();
        label2 = new Label();
        label3 = new Label();
        numeric_modPerc = new NumericUpDown();
        numeric_diceModificator = new NumericUpDown();
        numeric_diceSize = new NumericUpDown();
        numeric_diceCount = new NumericUpDown();
        label4 = new Label();
        textBox_diceResult = new TextBox();
        ((System.ComponentModel.ISupportInitialize)numeric_value).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_mod).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_modPerc).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_diceModificator).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_diceSize).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_diceCount).BeginInit();
        SuspendLayout();
        // 
        // numeric_value
        // 
        numeric_value.BackColor = Color.Black;
        numeric_value.ForeColor = Color.Lime;
        numeric_value.Location = new Point(14, 62);
        numeric_value.Margin = new Padding(5, 6, 5, 6);
        numeric_value.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_value.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numeric_value.Name = "numeric_value";
        numeric_value.Size = new Size(132, 36);
        numeric_value.TabIndex = 0;
        numeric_value.TextAlign = HorizontalAlignment.Right;
        numeric_value.Value = new decimal(new int[] { 10, 0, 0, 0 });
        numeric_value.ValueChanged += numericUpDown1_ValueChanged;
        // 
        // textBox_Results
        // 
        textBox_Results.BackColor = Color.Black;
        textBox_Results.Font = new Font("Segoe UI", 12F);
        textBox_Results.ForeColor = Color.Lime;
        textBox_Results.Location = new Point(17, 108);
        textBox_Results.Margin = new Padding(4);
        textBox_Results.Multiline = true;
        textBox_Results.Name = "textBox_Results";
        textBox_Results.ScrollBars = ScrollBars.Both;
        textBox_Results.Size = new Size(491, 462);
        textBox_Results.TabIndex = 1;
        // 
        // numeric_mod
        // 
        numeric_mod.BackColor = Color.Black;
        numeric_mod.ForeColor = Color.Lime;
        numeric_mod.Location = new Point(156, 62);
        numeric_mod.Margin = new Padding(5, 6, 5, 6);
        numeric_mod.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numeric_mod.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
        numeric_mod.Name = "numeric_mod";
        numeric_mod.Size = new Size(115, 36);
        numeric_mod.TabIndex = 2;
        numeric_mod.TextAlign = HorizontalAlignment.Right;
        numeric_mod.ValueChanged += numeric_mod_ValueChanged;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.ForeColor = Color.Lime;
        label1.Location = new Point(12, 26);
        label1.Name = "label1";
        label1.Size = new Size(134, 30);
        label1.TabIndex = 3;
        label1.Text = "Ожидаемое";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.ForeColor = Color.Lime;
        label2.Location = new Point(156, 26);
        label2.Name = "label2";
        label2.Size = new Size(58, 30);
        label2.TabIndex = 4;
        label2.Text = "Мод";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.ForeColor = Color.Lime;
        label3.Location = new Point(281, 26);
        label3.Name = "label3";
        label3.Size = new Size(96, 30);
        label3.TabIndex = 6;
        label3.Text = "Мод (%)";
        // 
        // numeric_modPerc
        // 
        numeric_modPerc.BackColor = Color.Black;
        numeric_modPerc.ForeColor = Color.Lime;
        numeric_modPerc.Location = new Point(281, 62);
        numeric_modPerc.Margin = new Padding(5, 6, 5, 6);
        numeric_modPerc.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numeric_modPerc.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
        numeric_modPerc.Name = "numeric_modPerc";
        numeric_modPerc.Size = new Size(83, 36);
        numeric_modPerc.TabIndex = 5;
        numeric_modPerc.TextAlign = HorizontalAlignment.Right;
        numeric_modPerc.Value = new decimal(new int[] { 50, 0, 0, 0 });
        // 
        // numeric_diceModificator
        // 
        numeric_diceModificator.BackColor = Color.Black;
        numeric_diceModificator.ForeColor = Color.Lime;
        numeric_diceModificator.Location = new Point(236, 622);
        numeric_diceModificator.Margin = new Padding(5, 6, 5, 6);
        numeric_diceModificator.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numeric_diceModificator.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
        numeric_diceModificator.Name = "numeric_diceModificator";
        numeric_diceModificator.Size = new Size(214, 36);
        numeric_diceModificator.TabIndex = 9;
        numeric_diceModificator.TextAlign = HorizontalAlignment.Right;
        numeric_diceModificator.ValueChanged += numeric_diceModificator_ValueChanged;
        numeric_diceModificator.Enter += numeric_diceModificator_Enter;
        // 
        // numeric_diceSize
        // 
        numeric_diceSize.BackColor = Color.Black;
        numeric_diceSize.ForeColor = Color.Lime;
        numeric_diceSize.Location = new Point(111, 622);
        numeric_diceSize.Margin = new Padding(5, 6, 5, 6);
        numeric_diceSize.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        numeric_diceSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numeric_diceSize.Name = "numeric_diceSize";
        numeric_diceSize.Size = new Size(115, 36);
        numeric_diceSize.TabIndex = 8;
        numeric_diceSize.TextAlign = HorizontalAlignment.Right;
        numeric_diceSize.Value = new decimal(new int[] { 5, 0, 0, 0 });
        numeric_diceSize.ValueChanged += numeric_diceSize_ValueChanged;
        numeric_diceSize.Enter += numeric_diceSize_Enter;
        // 
        // numeric_diceCount
        // 
        numeric_diceCount.BackColor = Color.Black;
        numeric_diceCount.ForeColor = Color.Lime;
        numeric_diceCount.Location = new Point(12, 622);
        numeric_diceCount.Margin = new Padding(5, 6, 5, 6);
        numeric_diceCount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_diceCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numeric_diceCount.Name = "numeric_diceCount";
        numeric_diceCount.Size = new Size(89, 36);
        numeric_diceCount.TabIndex = 7;
        numeric_diceCount.TextAlign = HorizontalAlignment.Right;
        numeric_diceCount.Value = new decimal(new int[] { 2, 0, 0, 0 });
        numeric_diceCount.ValueChanged += numeric_diceCount_ValueChanged;
        numeric_diceCount.Click += numeric_diceCount_Click;
        numeric_diceCount.Enter += numeric_diceCount_Enter;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.ForeColor = Color.Lime;
        label4.Location = new Point(14, 586);
        label4.Name = "label4";
        label4.Size = new Size(121, 30);
        label4.TabIndex = 10;
        label4.Text = "Dice число";
        // 
        // textBox_diceResult
        // 
        textBox_diceResult.BackColor = Color.Black;
        textBox_diceResult.Font = new Font("Segoe UI", 12F);
        textBox_diceResult.ForeColor = Color.Lime;
        textBox_diceResult.Location = new Point(12, 668);
        textBox_diceResult.Margin = new Padding(4);
        textBox_diceResult.Multiline = true;
        textBox_diceResult.Name = "textBox_diceResult";
        textBox_diceResult.ScrollBars = ScrollBars.Both;
        textBox_diceResult.Size = new Size(491, 205);
        textBox_diceResult.TabIndex = 11;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(12F, 30F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Black;
        ClientSize = new Size(521, 886);
        Controls.Add(textBox_diceResult);
        Controls.Add(label4);
        Controls.Add(numeric_diceModificator);
        Controls.Add(numeric_diceSize);
        Controls.Add(numeric_diceCount);
        Controls.Add(label3);
        Controls.Add(numeric_modPerc);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(numeric_mod);
        Controls.Add(textBox_Results);
        Controls.Add(numeric_value);
        Font = new Font("Segoe UI", 16F);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(5, 6, 5, 6);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "DiceCombinationHelper";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)numeric_value).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_mod).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_modPerc).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_diceModificator).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_diceSize).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_diceCount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private NumericUpDown numeric_value;
    private TextBox textBox_Results;
    private NumericUpDown numeric_mod;
    private Label label1;
    private Label label2;
    private Label label3;
    private NumericUpDown numeric_modPerc;
    private NumericUpDown numeric_diceModificator;
    private NumericUpDown numeric_diceSize;
    private NumericUpDown numeric_diceCount;
    private Label label4;
    private TextBox textBox_diceResult;
}
