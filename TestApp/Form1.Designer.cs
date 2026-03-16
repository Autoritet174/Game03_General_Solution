namespace TestApp;

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
        numericUpDown1 = new NumericUpDown();
        textBox1 = new TextBox();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
        SuspendLayout();
        // 
        // numericUpDown1
        // 
        numericUpDown1.Font = new Font("Segoe UI", 16F);
        numericUpDown1.Location = new Point(52, 31);
        numericUpDown1.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
        numericUpDown1.Name = "numericUpDown1";
        numericUpDown1.Size = new Size(222, 36);
        numericUpDown1.TabIndex = 0;
        numericUpDown1.TextAlign = HorizontalAlignment.Right;
        numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
        // 
        // textBox1
        // 
        textBox1.Font = new Font("Segoe UI", 16F);
        textBox1.Location = new Point(52, 90);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(222, 36);
        textBox1.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(textBox1);
        Controls.Add(numericUpDown1);
        Name = "Form1";
        Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private NumericUpDown numericUpDown1;
    private TextBox textBox1;
}
