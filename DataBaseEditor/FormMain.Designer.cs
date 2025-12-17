using Server_DB_Data.Entities.GameData__Lists;

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
        id = new DataGridViewTextBoxColumn();
        nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        nameRuDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        xWeaponTypeDamageTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        damageTypesDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
        weaponTypeBindingSource = new BindingSource(components);
        tabControl1 = new TabControl();
        tabPage_DamageTypes = new TabPage();
        button_WeaponTypes_Save = new Button();
        dgv_DamageTypes = new DataGridView();
        id1 = new DataGridViewTextBoxColumn();
        ColumnNameRu = new DataGridViewTextBoxColumn();
        ColumnDamageCoef = new DataGridViewTextBoxColumn();
        button_WeaponTypes_Refresh = new Button();
        tabPage2 = new TabPage();
        damageTypeBindingSource = new BindingSource(components);
        ((System.ComponentModel.ISupportInitialize)dgv_WeaponTypes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)weaponTypeBindingSource).BeginInit();
        tabControl1.SuspendLayout();
        tabPage_DamageTypes.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv_DamageTypes).BeginInit();
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
        dgv_WeaponTypes.Columns.AddRange(new DataGridViewColumn[] { id, nameDataGridViewTextBoxColumn, nameRuDataGridViewTextBoxColumn, xWeaponTypeDamageTypeDataGridViewTextBoxColumn, damageTypesDataGridViewTextBoxColumn });
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
        // id
        // 
        id.DataPropertyName = "Id";
        id.HeaderText = "Id";
        id.Name = "id";
        id.ReadOnly = true;
        // 
        // nameDataGridViewTextBoxColumn
        // 
        nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
        nameDataGridViewTextBoxColumn.HeaderText = "Name";
        nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
        nameDataGridViewTextBoxColumn.ReadOnly = true;
        nameDataGridViewTextBoxColumn.Visible = false;
        // 
        // nameRuDataGridViewTextBoxColumn
        // 
        nameRuDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        nameRuDataGridViewTextBoxColumn.DataPropertyName = "NameRu";
        nameRuDataGridViewTextBoxColumn.HeaderText = "NameRu";
        nameRuDataGridViewTextBoxColumn.Name = "nameRuDataGridViewTextBoxColumn";
        nameRuDataGridViewTextBoxColumn.ReadOnly = true;
        // 
        // xWeaponTypeDamageTypeDataGridViewTextBoxColumn
        // 
        xWeaponTypeDamageTypeDataGridViewTextBoxColumn.DataPropertyName = "X_WeaponType_DamageType";
        xWeaponTypeDamageTypeDataGridViewTextBoxColumn.HeaderText = "X_WeaponType_DamageType";
        xWeaponTypeDamageTypeDataGridViewTextBoxColumn.Name = "xWeaponTypeDamageTypeDataGridViewTextBoxColumn";
        xWeaponTypeDamageTypeDataGridViewTextBoxColumn.ReadOnly = true;
        xWeaponTypeDamageTypeDataGridViewTextBoxColumn.Visible = false;
        // 
        // damageTypesDataGridViewTextBoxColumn
        // 
        damageTypesDataGridViewTextBoxColumn.DataPropertyName = "DamageTypes";
        damageTypesDataGridViewTextBoxColumn.HeaderText = "DamageTypes";
        damageTypesDataGridViewTextBoxColumn.Name = "damageTypesDataGridViewTextBoxColumn";
        damageTypesDataGridViewTextBoxColumn.ReadOnly = true;
        damageTypesDataGridViewTextBoxColumn.Visible = false;
        // 
        // weaponTypeBindingSource
        // 
        weaponTypeBindingSource.DataSource = typeof(Server_DB_Data.Entities.__Lists.EquipmentType);
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage_DamageTypes);
        tabControl1.Controls.Add(tabPage2);
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
        tabPage_DamageTypes.Controls.Add(button_WeaponTypes_Save);
        tabPage_DamageTypes.Controls.Add(dgv_DamageTypes);
        tabPage_DamageTypes.Controls.Add(button_WeaponTypes_Refresh);
        tabPage_DamageTypes.Controls.Add(dgv_WeaponTypes);
        tabPage_DamageTypes.Location = new Point(4, 30);
        tabPage_DamageTypes.Margin = new Padding(4);
        tabPage_DamageTypes.Name = "tabPage_DamageTypes";
        tabPage_DamageTypes.Padding = new Padding(4);
        tabPage_DamageTypes.Size = new Size(1743, 850);
        tabPage_DamageTypes.TabIndex = 0;
        tabPage_DamageTypes.Text = "Типы оружия";
        tabPage_DamageTypes.UseVisualStyleBackColor = true;
        // 
        // button_WeaponTypes_Save
        // 
        button_WeaponTypes_Save.Location = new Point(112, 8);
        button_WeaponTypes_Save.Margin = new Padding(4);
        button_WeaponTypes_Save.Name = "button_WeaponTypes_Save";
        button_WeaponTypes_Save.Size = new Size(111, 32);
        button_WeaponTypes_Save.TabIndex = 3;
        button_WeaponTypes_Save.Text = "Сохранить";
        button_WeaponTypes_Save.UseVisualStyleBackColor = true;
        button_WeaponTypes_Save.Click += button_WeaponTypes_Save_Click;
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
        // button_WeaponTypes_Refresh
        // 
        button_WeaponTypes_Refresh.Location = new Point(8, 8);
        button_WeaponTypes_Refresh.Margin = new Padding(4);
        button_WeaponTypes_Refresh.Name = "button_WeaponTypes_Refresh";
        button_WeaponTypes_Refresh.Size = new Size(96, 32);
        button_WeaponTypes_Refresh.TabIndex = 1;
        button_WeaponTypes_Refresh.Text = "Обновить";
        button_WeaponTypes_Refresh.UseVisualStyleBackColor = true;
        button_WeaponTypes_Refresh.Click += button_WeaponTypes_Refresh_Click;
        // 
        // tabPage2
        // 
        tabPage2.Location = new Point(4, 24);
        tabPage2.Margin = new Padding(4);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new Padding(4);
        tabPage2.Size = new Size(1743, 856);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "tabPage2";
        tabPage2.UseVisualStyleBackColor = true;
        // 
        // damageTypeBindingSource
        // 
        damageTypeBindingSource.DataSource = typeof(DamageType);
        // 
        // FormMain
        // 
        AutoScaleDimensions = new SizeF(9F, 21F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1751, 884);
        Controls.Add(tabControl1);
        Font = new Font("Segoe UI", 12F);
        Margin = new Padding(4);
        Name = "FormMain";
        Text = "Редактор";
        ((System.ComponentModel.ISupportInitialize)dgv_WeaponTypes).EndInit();
        ((System.ComponentModel.ISupportInitialize)weaponTypeBindingSource).EndInit();
        tabControl1.ResumeLayout(false);
        tabPage_DamageTypes.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgv_DamageTypes).EndInit();
        ((System.ComponentModel.ISupportInitialize)damageTypeBindingSource).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv_WeaponTypes;
    private TabControl tabControl1;
    private TabPage tabPage_DamageTypes;
    private Button button_WeaponTypes_Refresh;
    private TabPage tabPage2;
    private BindingSource weaponTypeBindingSource;
    private DataGridView dgv_DamageTypes;
    private BindingSource damageTypeBindingSource;
    private DataGridViewTextBoxColumn id;
    private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn nameRuDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn xWeaponTypeDamageTypeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn damageTypesDataGridViewTextBoxColumn;
    private Button button_WeaponTypes_Save;
    private DataGridViewTextBoxColumn id1;
    private DataGridViewTextBoxColumn ColumnNameRu;
    private DataGridViewTextBoxColumn ColumnDamageCoef;
}
