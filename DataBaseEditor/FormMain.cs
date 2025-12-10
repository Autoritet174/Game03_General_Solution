using Microsoft.EntityFrameworkCore;
using Server_DB_Data;
using Server_DB_Data.Entities.X_Cross;

namespace DataBaseEditor;

public partial class FormMain : Form
{
    private const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=Game03_Data;Username=postgres;Password=";
    public FormMain()
    {
        InitializeComponent();
    }

    private void button_WeaponTypes_Refresh_Click(object sender, EventArgs e)
    {
        using var db = DbContext_Game03Data.Create(CONNECTION_STRING);
        dgv_WeaponTypes.AutoGenerateColumns = false;
        //dgv_WeaponTypes.Columns[ColumnNameRu.Name].DataPropertyName = "NameRu";
        dgv_WeaponTypes.DataSource = db.WeaponTypes.ToList();

    }

    private void dgv_WeaponTypes_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        int id = Convert.ToInt32(dgv_WeaponTypes.Rows[e.RowIndex].Cells["id"].Value);
        using var db = DbContext_Game03Data.Create(CONNECTION_STRING);
        var damageTypes = db.DamageTypes.Where(a => a.Id <= 4).ToList();
        dgv_DamageTypes.RowCount = damageTypes.Count;
        for (int i = 0; i < damageTypes.Count; i++)
        {
            dgv_DamageTypes.Rows[i].Cells["id1"].Value = damageTypes[i].Id;
            dgv_DamageTypes.Rows[i].Cells["ColumnNameRu"].Value = damageTypes[i].NameRu;

            int coef = db.X_WeaponTypes_DamageTypes.FirstOrDefault(a => a.DamageTypeId == damageTypes[i].Id && a.WeaponTypeId == id)?.DamageCoef ?? 0;

            dgv_DamageTypes.Rows[i].Cells["ColumnDamageCoef"].Value = coef;
        }
    }

    private void button_WeaponTypes_Save_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(dgv_WeaponTypes.CurrentRow!.Cells["id"].Value);
        using var db = DbContext_Game03Data.Create(CONNECTION_STRING);
        IQueryable<X_WeaponType_DamageType> xArray = db.X_WeaponTypes_DamageTypes.Where(a => a.WeaponTypeId == id);
        for (int i = 0; i < dgv_DamageTypes.RowCount; i++)
        {
            DataGridViewRow row = dgv_DamageTypes.Rows[i];
            int damageTypeId = Convert.ToInt32(row.Cells["id1"].Value);
            int damageCoef = Convert.ToInt32(row.Cells["ColumnDamageCoef"].Value);

            X_WeaponType_DamageType? x = xArray.FirstOrDefault(a => a.DamageTypeId == damageTypeId);
            if (x == null)
            {
                x = new()
                {
                    DamageTypeId = damageTypeId,
                    WeaponTypeId = id,
                    DamageCoef = damageCoef,
                    DamageType = db.DamageTypes.First(a => a.Id == damageTypeId),
                    WeaponType = db.WeaponTypes.First(a => a.Id == id),
                };
                _ = db.X_WeaponTypes_DamageTypes.Add(x);
            }
            else
            {
                x.DamageCoef = damageCoef;
            }
        }

        var xArrayForDelete = db.X_WeaponTypes_DamageTypes.Where(a => a.DamageCoef <= 0).ToList();
        foreach (X_WeaponType_DamageType? x in xArrayForDelete)
        {
            _ = db.X_WeaponTypes_DamageTypes.Remove(x);
        }
        _ = db.SaveChanges();
    }
}
