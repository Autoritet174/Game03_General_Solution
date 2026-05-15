namespace DataBaseEditor.Tables;

partial class UCDice
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
        label1 = new Label();
        numeric_count = new NumericUpDown();
        label2 = new Label();
        numeric_sides = new NumericUpDown();
        label3 = new Label();
        numeric_mod = new NumericUpDown();
        label4 = new Label();
        numeric_min = new NumericUpDown();
        label5 = new Label();
        numeric_max = new NumericUpDown();
        label6 = new Label();
        numeric_exc = new NumericUpDown();
        radioButton_csm = new RadioButton();
        radioButton_exc = new RadioButton();
        label_name = new Label();
        ((System.ComponentModel.ISupportInitialize)numeric_count).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_sides).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_mod).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_min).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_max).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numeric_exc).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(3, 21);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(49, 21);
        label1.TabIndex = 0;
        label1.Text = "count";
        // 
        // numeric_count
        // 
        numeric_count.Location = new Point(3, 41);
        numeric_count.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_count.Name = "numeric_count";
        numeric_count.Size = new Size(67, 29);
        numeric_count.TabIndex = 1;
        numeric_count.TextAlign = HorizontalAlignment.Right;
        numeric_count.Value = new decimal(new int[] { 10000, 0, 0, 0 });
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(71, 21);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(45, 21);
        label2.TabIndex = 0;
        label2.Text = "sides";
        // 
        // numeric_sides
        // 
        numeric_sides.Location = new Point(71, 41);
        numeric_sides.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_sides.Name = "numeric_sides";
        numeric_sides.Size = new Size(67, 29);
        numeric_sides.TabIndex = 2;
        numeric_sides.TextAlign = HorizontalAlignment.Right;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(139, 21);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new Size(42, 21);
        label3.TabIndex = 0;
        label3.Text = "mod";
        // 
        // numeric_mod
        // 
        numeric_mod.DecimalPlaces = 5;
        numeric_mod.Location = new Point(139, 41);
        numeric_mod.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_mod.Name = "numeric_mod";
        numeric_mod.Size = new Size(116, 29);
        numeric_mod.TabIndex = 3;
        numeric_mod.TextAlign = HorizontalAlignment.Right;
        numeric_mod.Value = new decimal(new int[] { 10000, 0, 0, 0 });
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(3, 67);
        label4.Margin = new Padding(4, 0, 4, 0);
        label4.Name = "label4";
        label4.Size = new Size(37, 21);
        label4.TabIndex = 0;
        label4.Text = "min";
        // 
        // numeric_min
        // 
        numeric_min.Enabled = false;
        numeric_min.Location = new Point(3, 87);
        numeric_min.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_min.Name = "numeric_min";
        numeric_min.Size = new Size(67, 29);
        numeric_min.TabIndex = 1;
        numeric_min.TextAlign = HorizontalAlignment.Right;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new Point(71, 67);
        label5.Margin = new Padding(4, 0, 4, 0);
        label5.Name = "label5";
        label5.Size = new Size(39, 21);
        label5.TabIndex = 0;
        label5.Text = "max";
        // 
        // numeric_max
        // 
        numeric_max.Enabled = false;
        numeric_max.Location = new Point(71, 87);
        numeric_max.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_max.Name = "numeric_max";
        numeric_max.Size = new Size(67, 29);
        numeric_max.TabIndex = 1;
        numeric_max.TextAlign = HorizontalAlignment.Right;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new Point(139, 67);
        label6.Margin = new Padding(4, 0, 4, 0);
        label6.Name = "label6";
        label6.Size = new Size(71, 21);
        label6.TabIndex = 0;
        label6.Text = "excepted";
        // 
        // numeric_exc
        // 
        numeric_exc.DecimalPlaces = 5;
        numeric_exc.Location = new Point(139, 87);
        numeric_exc.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numeric_exc.Name = "numeric_exc";
        numeric_exc.Size = new Size(116, 29);
        numeric_exc.TabIndex = 4;
        numeric_exc.TextAlign = HorizontalAlignment.Right;
        // 
        // radioButton_csm
        // 
        radioButton_csm.AutoSize = true;
        radioButton_csm.Location = new Point(258, 47);
        radioButton_csm.Name = "radioButton_csm";
        radioButton_csm.Size = new Size(14, 13);
        radioButton_csm.TabIndex = 2;
        radioButton_csm.UseVisualStyleBackColor = true;
        radioButton_csm.CheckedChanged += radioButton_csm_CheckedChanged;
        // 
        // radioButton_exc
        // 
        radioButton_exc.AutoSize = true;
        radioButton_exc.Checked = true;
        radioButton_exc.Location = new Point(258, 93);
        radioButton_exc.Name = "radioButton_exc";
        radioButton_exc.Size = new Size(14, 13);
        radioButton_exc.TabIndex = 2;
        radioButton_exc.TabStop = true;
        radioButton_exc.UseVisualStyleBackColor = true;
        radioButton_exc.CheckedChanged += radioButton_exc_CheckedChanged;
        // 
        // label_name
        // 
        label_name.AutoSize = true;
        label_name.BackColor = Color.FromArgb(192, 255, 192);
        label_name.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        label_name.Location = new Point(3, 3);
        label_name.Margin = new Padding(4, 0, 4, 0);
        label_name.Name = "label_name";
        label_name.Size = new Size(56, 21);
        label_name.TabIndex = 5;
        label_name.Text = "Name";
        // 
        // UCDice
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(label_name);
        Controls.Add(radioButton_exc);
        Controls.Add(radioButton_csm);
        Controls.Add(numeric_exc);
        Controls.Add(numeric_mod);
        Controls.Add(label3);
        Controls.Add(numeric_sides);
        Controls.Add(label2);
        Controls.Add(numeric_max);
        Controls.Add(numeric_min);
        Controls.Add(numeric_count);
        Controls.Add(label1);
        Controls.Add(label6);
        Controls.Add(label5);
        Controls.Add(label4);
        Font = new Font("Segoe UI", 12F);
        Margin = new Padding(4);
        Name = "UCDice";
        Size = new Size(285, 121);
        ((System.ComponentModel.ISupportInitialize)numeric_count).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_sides).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_mod).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_min).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_max).EndInit();
        ((System.ComponentModel.ISupportInitialize)numeric_exc).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private NumericUpDown numeric_count;
    private Label label2;
    private NumericUpDown numeric_sides;
    private Label label3;
    private NumericUpDown numeric_mod;
    private Label label4;
    private NumericUpDown numeric_min;
    private Label label5;
    private NumericUpDown numeric_max;
    private Label label6;
    private NumericUpDown numeric_exc;
    private RadioButton radioButton_csm;
    private RadioButton radioButton_exc;
    private Label label_name;
}
