namespace GameLauncher;

partial class FormMain
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
        components = new System.ComponentModel.Container();
        kryptonManager1 = new Krypton.Toolkit.KryptonManager(components);
        SuspendLayout();
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(187, 206, 230);
        ClientSize = new Size(1266, 555);
        Font = new Font("Segoe UI", 12F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(4);
        MaximizeBox = false;
        MaximumSize = new Size(1280, 600);
        MinimumSize = new Size(1280, 600);
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Game launcher";
        Load += Form1_Load;
        ResumeLayout(false);
    }

    #endregion
    private Krypton.Toolkit.KryptonManager kryptonManager1;
}
