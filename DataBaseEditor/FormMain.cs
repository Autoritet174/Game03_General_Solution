using DataBaseEditor.Tables;
using General.DTO.Entities.GameData;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using Server_DB_Postgres;

namespace DataBaseEditor;

public partial class FormMain : Form
{
    #region Form


    public FormMain()
    {
        InitializeComponent();
        dgv_BaseHeroes.AutoGenerateColumns = true;
    }
    #endregion
    #region WeaponTypes
    private void button_Refresh_WeaponTypes_Click(object sender, EventArgs e)
    {
        using DbContextGame db = DB.Create();
        dgv_WeaponTypes.AutoGenerateColumns = false;
        //dgv_WeaponTypes.Columns[ColumnNameRu.Name].DataPropertyName = "NameRu";
        dgv_WeaponTypes.DataSource = db.EquipmentTypes.ToList();
    }
    private void dgv_WeaponTypes_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        int id = Convert.ToInt32(dgv_WeaponTypes.Rows[e.RowIndex].Cells["id"].Value);
        using DbContextGame db = DB.Create();
        var damageTypes = db.DamageTypes.Where(a => a.Id <= 4).ToList();
        dgv_DamageTypes.RowCount = damageTypes.Count;
        for (int i = 0; i < damageTypes.Count; i++)
        {
            dgv_DamageTypes.Rows[i].Cells["id1"].Value = damageTypes[i].Id;
            dgv_DamageTypes.Rows[i].Cells["ColumnNameRu"].Value = damageTypes[i].NameRu;

            int coef = db.x_EquipmentTypes_DamageTypes.FirstOrDefault(a => a.DamageTypeId == damageTypes[i].Id && a.EquipmentTypeId == id)?.DamageCoef ?? 0;

            dgv_DamageTypes.Rows[i].Cells["ColumnDamageCoef"].Value = coef;
        }
    }
    private void button_Save_WeaponTypes_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(dgv_WeaponTypes.CurrentRow!.Cells["id"].Value);
        using DbContextGame db = DB.Create();
        IQueryable<X_EquipmentType_DamageType> xArray = db.x_EquipmentTypes_DamageTypes.Where(a => a.EquipmentTypeId == id);
        for (int i = 0; i < dgv_DamageTypes.RowCount; i++)
        {
            DataGridViewRow row = dgv_DamageTypes.Rows[i];
            int damageTypeId = Convert.ToInt32(row.Cells["id1"].Value);
            int damageCoef = Convert.ToInt32(row.Cells["ColumnDamageCoef"].Value);

            X_EquipmentType_DamageType? x = xArray.FirstOrDefault(a => a.DamageTypeId == damageTypeId);
            if (x == null)
            {
                x = new()
                {
                    DamageTypeId = damageTypeId,
                    EquipmentTypeId = id,
                    DamageCoef = damageCoef,
                    DamageType = db.DamageTypes.First(a => a.Id == damageTypeId),
                    EquipmentType = db.EquipmentTypes.First(a => a.Id == id),
                };
                _ = db.x_EquipmentTypes_DamageTypes.Add(x);
            }
            else
            {
                x.DamageCoef = damageCoef;
            }
        }

        var xArrayForDelete = db.x_EquipmentTypes_DamageTypes.Where(a => a.DamageCoef <= 0).ToList();
        foreach (X_EquipmentType_DamageType? x in xArrayForDelete)
        {
            _ = db.x_EquipmentTypes_DamageTypes.Remove(x);
        }
        _ = db.SaveChanges();
    }
    #endregion
    #region BaseHeroes
    void RefreshData_BaseHeroes() {
        using DbContextGame db = DB.Create();
        var list = db.BaseHeroes.OrderBy(a => a.Rarity).ThenBy(a => a.Id).ToList();
        var list2 = list.Select(a => new
        {
            a.Id,
            a.Name,
            a.Rarity,
            a.MainStat,
            Health = a.Health.Expected,
            Damage = a.Damage.Expected,
            H_D = a.Health.Expected / a.Damage.Expected
        }).ToList();
        dgv_BaseHeroes.DataSource = list2;
    }
    private void button_Refresh_BaseHeroes_Click(object sender, EventArgs e)
    {
        RefreshData_BaseHeroes();
    }
    private void button_Save_BaseHeroes_Click(object sender, EventArgs e)
    {

    }
    #endregion
    private void FormMain_Load(object sender, EventArgs e)
    {

    }

    private void dgv_BaseHeroes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            DataGridViewRow row = dgv_BaseHeroes.Rows[e.RowIndex];
            if (row != null)
            {
                int id = Convert.ToInt32(row.Cells[0].Value);
                if (id > 0)
                {
                    new BaseHeroesForm(id).ShowDialog();
                    RefreshData_BaseHeroes();
                }
            }
        }
    }
}
