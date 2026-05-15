
namespace DataBaseEditor;

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
        dgv_WeaponTypes = new DataGridView();
        weaponTypeBindingSource = new BindingSource(components);
        tabControl1 = new TabControl();
        tabPage_DamageTypes = new TabPage();
        panel2 = new Panel();
        button_Refresh_WeaponTypes = new Button();
        button_Save_WeaponTypes = new Button();
        dgv_DamageTypes = new DataGridView();
        id1 = new DataGridViewTextBoxColumn();
        ColumnNameRu = new DataGridViewTextBoxColumn();
        ColumnDamageCoef = new DataGridViewTextBoxColumn();
        tabPage_BaseHero = new TabPage();
        dgv_BaseHeroes = new DataGridView();
        panel1 = new Panel();
        button_Refresh_BaseHeroes = new Button();
        button_Save_BaseHeroes = new Button();
        damageTypeBindingSource = new BindingSource(components);
        ((System.ComponentModel.ISupportInitialize)dgv_WeaponTypes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)weaponTypeBindingSource).BeginInit();
        tabControl1.SuspendLayout();
        tabPage_DamageTypes.SuspendLayout();
        panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv_DamageTypes).BeginInit();
        tabPage_BaseHero.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv_BaseHeroes).BeginInit();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)damageTypeBindingSource).BeginInit();
        SuspendLayout();
        // 
        // dgv_WeaponTypes
        // 
        dgv_WeaponTypes.AllowUserToAddRows = false;
        dgv_WeaponTypes.AllowUserToDeleteRows = false;
        dgv_WeaponTypes.AllowUserToResizeColumns = false;
        dgv_WeaponTypes.AllowUserToResizeRows = false;
        dgv_WeaponTypes.AutoGenerateColumns = false;
        dgv_WeaponTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv_WeaponTypes.DataSource = weaponTypeBindingSource;
        dgv_WeaponTypes.Location = new Point(4, 48);
        dgv_WeaponTypes.Margin = new Padding(4);
        dgv_WeaponTypes.Name = "dgv_WeaponTypes";
        dgv_WeaponTypes.ReadOnly = true;
        dgv_WeaponTypes.RowHeadersVisible = false;
        dgv_WeaponTypes.Size = new Size(357, 593);
        dgv_WeaponTypes.TabIndex = 0;
        dgv_WeaponTypes.CellClick += dgv_WeaponTypes_CellClick;
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage_DamageTypes);
        tabControl1.Controls.Add(tabPage_BaseHero);
        tabControl1.Dock = DockStyle.Fill;
        tabControl1.Location = new Point(0, 0);
        tabControl1.Margin = new Padding(4);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(1751, 884);
        tabControl1.TabIndex = 1;
        // 
        // tabPage_DamageTypes
        // 
        tabPage_DamageTypes.BackColor = Color.Black;
        tabPage_DamageTypes.Controls.Add(panel2);
        tabPage_DamageTypes.Controls.Add(dgv_DamageTypes);
        tabPage_DamageTypes.Controls.Add(dgv_WeaponTypes);
        tabPage_DamageTypes.Location = new Point(4, 30);
        tabPage_DamageTypes.Margin = new Padding(4);
        tabPage_DamageTypes.Name = "tabPage_DamageTypes";
        tabPage_DamageTypes.Padding = new Padding(4);
        tabPage_DamageTypes.Size = new Size(1743, 850);
        tabPage_DamageTypes.TabIndex = 0;
        tabPage_DamageTypes.Text = "Типы оружия";
        // 
        // panel2
        // 
        panel2.Controls.Add(button_Refresh_WeaponTypes);
        panel2.Controls.Add(button_Save_WeaponTypes);
        panel2.Dock = DockStyle.Top;
        panel2.Location = new Point(4, 4);
        panel2.Name = "panel2";
        panel2.Size = new Size(1735, 40);
        panel2.TabIndex = 8;
        // 
        // button_Refresh_WeaponTypes
        // 
        button_Refresh_WeaponTypes.Location = new Point(4, 4);
        button_Refresh_WeaponTypes.Margin = new Padding(4);
        button_Refresh_WeaponTypes.Name = "button_Refresh_WeaponTypes";
        button_Refresh_WeaponTypes.Size = new Size(96, 32);
        button_Refresh_WeaponTypes.TabIndex = 1;
        button_Refresh_WeaponTypes.Text = "Обновить";
        button_Refresh_WeaponTypes.UseVisualStyleBackColor = true;
        button_Refresh_WeaponTypes.Click += button_Refresh_WeaponTypes_Click;
        // 
        // button_Save_WeaponTypes
        // 
        button_Save_WeaponTypes.Location = new Point(108, 4);
        button_Save_WeaponTypes.Margin = new Padding(4);
        button_Save_WeaponTypes.Name = "button_Save_WeaponTypes";
        button_Save_WeaponTypes.Size = new Size(111, 32);
        button_Save_WeaponTypes.TabIndex = 3;
        button_Save_WeaponTypes.Text = "Сохранить";
        button_Save_WeaponTypes.UseVisualStyleBackColor = true;
        button_Save_WeaponTypes.Click += button_Save_WeaponTypes_Click;
        // 
        // dgv_DamageTypes
        // 
        dgv_DamageTypes.AllowUserToAddRows = false;
        dgv_DamageTypes.AllowUserToDeleteRows = false;
        dgv_DamageTypes.AllowUserToResizeColumns = false;
        dgv_DamageTypes.AllowUserToResizeRows = false;
        dgv_DamageTypes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv_DamageTypes.Columns.AddRange(new DataGridViewColumn[] { id1, ColumnNameRu, ColumnDamageCoef });
        dgv_DamageTypes.Location = new Point(369, 48);
        dgv_DamageTypes.Margin = new Padding(4);
        dgv_DamageTypes.Name = "dgv_DamageTypes";
        dgv_DamageTypes.RowHeadersVisible = false;
        dgv_DamageTypes.Size = new Size(612, 593);
        dgv_DamageTypes.TabIndex = 2;
        // 
        // id1
        // 
        id1.HeaderText = "id";
        id1.Name = "id1";
        // 
        // ColumnNameRu
        // 
        ColumnNameRu.HeaderText = "NameRu";
        ColumnNameRu.Name = "ColumnNameRu";
        ColumnNameRu.Width = 200;
        // 
        // ColumnDamageCoef
        // 
        ColumnDamageCoef.HeaderText = "DamageCoef";
        ColumnDamageCoef.Name = "ColumnDamageCoef";
        ColumnDamageCoef.Width = 120;
        // 
        // tabPage_BaseHero
        // 
        tabPage_BaseHero.BackColor = Color.Black;
        tabPage_BaseHero.Controls.Add(dgv_BaseHeroes);
        tabPage_BaseHero.Controls.Add(panel1);
        tabPage_BaseHero.Location = new Point(4, 24);
        tabPage_BaseHero.Margin = new Padding(4);
        tabPage_BaseHero.Name = "tabPage_BaseHero";
        tabPage_BaseHero.Padding = new Padding(4);
        tabPage_BaseHero.Size = new Size(1743, 856);
        tabPage_BaseHero.TabIndex = 1;
        tabPage_BaseHero.Text = "Базовые герои";
        // 
        // dgv_BaseHeroes
        // 
        dgv_BaseHeroes.AllowUserToAddRows = false;
        dgv_BaseHeroes.AllowUserToDeleteRows = false;
        dgv_BaseHeroes.AllowUserToResizeColumns = false;
        dgv_BaseHeroes.AllowUserToResizeRows = false;
        dgv_BaseHeroes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv_BaseHeroes.Dock = DockStyle.Fill;
        dgv_BaseHeroes.Location = new Point(4, 44);
        dgv_BaseHeroes.Margin = new Padding(4);
        dgv_BaseHeroes.Name = "dgv_BaseHeroes";
        dgv_BaseHeroes.RowHeadersVisible = false;
        dgv_BaseHeroes.Size = new Size(1735, 808);
        dgv_BaseHeroes.TabIndex = 6;
        // 
        // panel1
        // 
        panel1.Controls.Add(button_Refresh_BaseHeroes);
        panel1.Controls.Add(button_Save_BaseHeroes);
        panel1.Dock = DockStyle.Top;
        panel1.Location = new Point(4, 4);
        panel1.Name = "panel1";
        panel1.Size = new Size(1735, 40);
        panel1.TabIndex = 7;
        // 
        // button_Refresh_BaseHeroes
        // 
        button_Refresh_BaseHeroes.Location = new Point(4, 4);
        button_Refresh_BaseHeroes.Margin = new Padding(4);
        button_Refresh_BaseHeroes.Name = "button_Refresh_BaseHeroes";
        button_Refresh_BaseHeroes.Size = new Size(96, 32);
        button_Refresh_BaseHeroes.TabIndex = 4;
        button_Refresh_BaseHeroes.Text = "Обновить";
        button_Refresh_BaseHeroes.UseVisualStyleBackColor = true;
        button_Refresh_BaseHeroes.Click += button_Refresh_BaseHeroes_Click;
        // 
        // button_Save_BaseHeroes
        // 
        button_Save_BaseHeroes.Location = new Point(108, 4);
        button_Save_BaseHeroes.Margin = new Padding(4);
        button_Save_BaseHeroes.Name = "button_Save_BaseHeroes";
        button_Save_BaseHeroes.Size = new Size(111, 32);
        button_Save_BaseHeroes.TabIndex = 5;
        button_Save_BaseHeroes.Text = "Сохранить";
        button_Save_BaseHeroes.UseVisualStyleBackColor = true;
        button_Save_BaseHeroes.Click += button_Save_BaseHeroes_Click;
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Black;
        ClientSize = new Size(1751, 884);
        Controls.Add(tabControl1);
        Font = new Font("Segoe UI", 12F);
        Margin = new Padding(4);
        Name = "FormMain";
        Text = "Редактор";
        Load += FormMain_Load;
        ((System.ComponentModel.ISupportInitialize)dgv_WeaponTypes).EndInit();
        ((System.ComponentModel.ISupportInitialize)weaponTypeBindingSource).EndInit();
        tabControl1.ResumeLayout(false);
        tabPage_DamageTypes.ResumeLayout(false);
        panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgv_DamageTypes).EndInit();
        tabPage_BaseHero.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgv_BaseHeroes).EndInit();
        panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)damageTypeBindingSource).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv_WeaponTypes;
    private TabControl tabControl1;
    private TabPage tabPage_DamageTypes;
    private Button button_Refresh_WeaponTypes;
    private TabPage tabPage_BaseHero;
    private BindingSource weaponTypeBindingSource;
    private DataGridView dgv_DamageTypes;
    private BindingSource damageTypeBindingSource;
    private DataGridViewTextBoxColumn id;
    private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn nameRuDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn xWeaponTypeDamageTypeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn damageTypesDataGridViewTextBoxColumn;
    private Button button_Save_WeaponTypes;
    private DataGridViewTextBoxColumn id1;
    private DataGridViewTextBoxColumn ColumnNameRu;
    private DataGridViewTextBoxColumn ColumnDamageCoef;
    private Button button_Save_BaseHeroes;
    private Button button_Refresh_BaseHeroes;
    private DataGridView dgv_BaseHeroes;
    private Panel panel2;
    private Panel panel1;
}
