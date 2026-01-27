namespace BackuperDatabases;

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
        label1 = new Label();
        button_Backup_PostgreSql_Users = new Button();
        button_Restore_PostgreSql_Users = new Button();
        label2 = new Label();
        button_Backup_PostgreSql_GameData = new Button();
        button_Restore_PostgreSql_GameData = new Button();
        label3 = new Label();
        button_Backup_MongoDb_UserData = new Button();
        button_Restore_MongoDb_UserData = new Button();
        button_Backup_All = new Button();
        button_Restore_All = new Button();
        button_RestoreFromLast_PostgreSql_Users = new Button();
        label_Complete = new Label();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(8, 19);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(84, 21);
        label1.TabIndex = 0;
        label1.Text = "PostgreSql";
        // 
        // button_Backup_PostgreSql_Users
        // 
        button_Backup_PostgreSql_Users.Location = new Point(180, 13);
        button_Backup_PostgreSql_Users.Margin = new Padding(4);
        button_Backup_PostgreSql_Users.Name = "button_Backup_PostgreSql_Users";
        button_Backup_PostgreSql_Users.Size = new Size(96, 32);
        button_Backup_PostgreSql_Users.TabIndex = 1;
        button_Backup_PostgreSql_Users.Text = "Backup";
        button_Backup_PostgreSql_Users.UseVisualStyleBackColor = true;
        button_Backup_PostgreSql_Users.Click += button_Backup_PostgreSql_Users_Click;
        // 
        // button_Restore_PostgreSql_Users
        // 
        button_Restore_PostgreSql_Users.Location = new Point(284, 13);
        button_Restore_PostgreSql_Users.Margin = new Padding(4);
        button_Restore_PostgreSql_Users.Name = "button_Restore_PostgreSql_Users";
        button_Restore_PostgreSql_Users.Size = new Size(96, 32);
        button_Restore_PostgreSql_Users.TabIndex = 1;
        button_Restore_PostgreSql_Users.Text = "Restore";
        button_Restore_PostgreSql_Users.UseVisualStyleBackColor = true;
        button_Restore_PostgreSql_Users.Click += button_Restore_PostgreSql_Users_Click;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(8, 59);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        label2.Size = new Size(123, 21);
        label2.TabIndex = 0;
        label2.Text = "PostgreSql_Data";
        label2.Visible = false;
        // 
        // button_Backup_PostgreSql_GameData
        // 
        button_Backup_PostgreSql_GameData.Location = new Point(180, 53);
        button_Backup_PostgreSql_GameData.Margin = new Padding(4);
        button_Backup_PostgreSql_GameData.Name = "button_Backup_PostgreSql_GameData";
        button_Backup_PostgreSql_GameData.Size = new Size(96, 32);
        button_Backup_PostgreSql_GameData.TabIndex = 1;
        button_Backup_PostgreSql_GameData.Text = "Backup";
        button_Backup_PostgreSql_GameData.UseVisualStyleBackColor = true;
        button_Backup_PostgreSql_GameData.Visible = false;
        button_Backup_PostgreSql_GameData.Click += button_Backup_PostgreSql_GameData_Click;
        // 
        // button_Restore_PostgreSql_GameData
        // 
        button_Restore_PostgreSql_GameData.Location = new Point(284, 53);
        button_Restore_PostgreSql_GameData.Margin = new Padding(4);
        button_Restore_PostgreSql_GameData.Name = "button_Restore_PostgreSql_GameData";
        button_Restore_PostgreSql_GameData.Size = new Size(96, 32);
        button_Restore_PostgreSql_GameData.TabIndex = 1;
        button_Restore_PostgreSql_GameData.Text = "Restore";
        button_Restore_PostgreSql_GameData.UseVisualStyleBackColor = true;
        button_Restore_PostgreSql_GameData.Visible = false;
        button_Restore_PostgreSql_GameData.Click += button_Restore_PostgreSql_GameData_Click;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(8, 99);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new Size(151, 21);
        label3.TabIndex = 0;
        label3.Text = "MongoDb_UserData";
        label3.Visible = false;
        // 
        // button_Backup_MongoDb_UserData
        // 
        button_Backup_MongoDb_UserData.Location = new Point(180, 93);
        button_Backup_MongoDb_UserData.Margin = new Padding(4);
        button_Backup_MongoDb_UserData.Name = "button_Backup_MongoDb_UserData";
        button_Backup_MongoDb_UserData.Size = new Size(96, 32);
        button_Backup_MongoDb_UserData.TabIndex = 1;
        button_Backup_MongoDb_UserData.Text = "Backup";
        button_Backup_MongoDb_UserData.UseVisualStyleBackColor = true;
        button_Backup_MongoDb_UserData.Visible = false;
        button_Backup_MongoDb_UserData.Click += button_Backup_MongoDb_UserData_Click;
        // 
        // button_Restore_MongoDb_UserData
        // 
        button_Restore_MongoDb_UserData.Location = new Point(284, 93);
        button_Restore_MongoDb_UserData.Margin = new Padding(4);
        button_Restore_MongoDb_UserData.Name = "button_Restore_MongoDb_UserData";
        button_Restore_MongoDb_UserData.Size = new Size(96, 32);
        button_Restore_MongoDb_UserData.TabIndex = 1;
        button_Restore_MongoDb_UserData.Text = "Restore";
        button_Restore_MongoDb_UserData.UseVisualStyleBackColor = true;
        button_Restore_MongoDb_UserData.Visible = false;
        button_Restore_MongoDb_UserData.Click += button_Restore_MongoDb_UserData_Click;
        // 
        // button_Backup_All
        // 
        button_Backup_All.Location = new Point(180, 133);
        button_Backup_All.Margin = new Padding(4);
        button_Backup_All.Name = "button_Backup_All";
        button_Backup_All.Size = new Size(96, 32);
        button_Backup_All.TabIndex = 1;
        button_Backup_All.Text = "Backup all";
        button_Backup_All.UseVisualStyleBackColor = true;
        button_Backup_All.Visible = false;
        button_Backup_All.Click += button_Backup_All_Click;
        // 
        // button_Restore_All
        // 
        button_Restore_All.Location = new Point(284, 133);
        button_Restore_All.Margin = new Padding(4);
        button_Restore_All.Name = "button_Restore_All";
        button_Restore_All.Size = new Size(96, 32);
        button_Restore_All.TabIndex = 1;
        button_Restore_All.Text = "Restore all";
        button_Restore_All.UseVisualStyleBackColor = true;
        button_Restore_All.Visible = false;
        button_Restore_All.Click += button_Restore_All_Click;
        // 
        // button_RestoreFromLast_PostgreSql_Users
        // 
        button_RestoreFromLast_PostgreSql_Users.Location = new Point(388, 13);
        button_RestoreFromLast_PostgreSql_Users.Margin = new Padding(4);
        button_RestoreFromLast_PostgreSql_Users.Name = "button_RestoreFromLast_PostgreSql_Users";
        button_RestoreFromLast_PostgreSql_Users.Size = new Size(142, 32);
        button_RestoreFromLast_PostgreSql_Users.TabIndex = 2;
        button_RestoreFromLast_PostgreSql_Users.Text = "Restore from last";
        button_RestoreFromLast_PostgreSql_Users.UseVisualStyleBackColor = true;
        button_RestoreFromLast_PostgreSql_Users.Click += button_RestoreFromLast_PostgreSql_Users_Click;
        // 
        // label_Complete
        // 
        label_Complete.AutoSize = true;
        label_Complete.Location = new Point(388, 49);
        label_Complete.Margin = new Padding(4, 0, 4, 0);
        label_Complete.Name = "label_Complete";
        label_Complete.Size = new Size(77, 21);
        label_Complete.TabIndex = 3;
        label_Complete.Text = "Complete";
        label_Complete.Visible = false;
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(543, 178);
        Controls.Add(label_Complete);
        Controls.Add(button_RestoreFromLast_PostgreSql_Users);
        Controls.Add(button_Restore_All);
        Controls.Add(button_Restore_MongoDb_UserData);
        Controls.Add(button_Restore_PostgreSql_GameData);
        Controls.Add(button_Restore_PostgreSql_Users);
        Controls.Add(button_Backup_All);
        Controls.Add(button_Backup_MongoDb_UserData);
        Controls.Add(label3);
        Controls.Add(button_Backup_PostgreSql_GameData);
        Controls.Add(label2);
        Controls.Add(button_Backup_PostgreSql_Users);
        Controls.Add(label1);
        Font = new Font("Segoe UI", 12F);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(4);
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "BackuperDatabases";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Button button_Backup_PostgreSql_Users;
    private Button button_Restore_PostgreSql_Users;
    private Label label2;
    private Button button_Backup_PostgreSql_GameData;
    private Button button_Restore_PostgreSql_GameData;
    private Label label3;
    private Button button_Backup_MongoDb_UserData;
    private Button button_Restore_MongoDb_UserData;
    private Button button_Backup_All;
    private Button button_Restore_All;
    private Button button_RestoreFromLast_PostgreSql_Users;
    private Label label_Complete;
}
