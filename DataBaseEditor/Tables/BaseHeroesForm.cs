using General.DTO.Entities.GameData;
using Server_DB_Postgres;

namespace DataBaseEditor.Tables;

public partial class BaseHeroesForm : Form
{
    private readonly int id = 0;
    private readonly DbContextGame db = DB.Create();
    public BaseHeroesForm(int id)
    {
        this.id = id;
        InitializeComponent();
    }

    private void SaveData()
    {
        BaseHero baseHero = db.BaseHeroes.First(a => a.Id == id);
        try
        {
            baseHero.Health = ucDice_Health.GetValue();
            baseHero.Damage = ucDice_Damage.GetValue();
            _ = db.SaveChanges();
        } catch (Exception ex) {
            MessageBox.Show(ex.Message);
        }
    }

    private void RefreshData()
    {
        BaseHero baseHero = db.BaseHeroes.First(a => a.Id == id);
        ucDice_Health.SetValue(baseHero.Health);
        ucDice_Damage.SetValue(baseHero.Damage);
    }

    private void button_save_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    private void BaseHeroesForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        //SaveData();
        db.Dispose();
    }

    private void BaseHeroesForm_Load(object sender, EventArgs e)
    {
        BaseHero baseHero = db.BaseHeroes.First(a => a.Id == id);
        ucDice_Health.Init("Health", baseHero);
        ucDice_Damage.Init("Damage", baseHero);
        RefreshData();
    }
}
