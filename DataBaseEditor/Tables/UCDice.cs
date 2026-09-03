using General.DTO;
using General.DTO.Entities.GameData;

namespace DataBaseEditor.Tables;

public partial class UCDice : UserControl
{
    private BaseHero? baseHero;
    private string name = "";
    public UCDice()
    {
        InitializeComponent();
    }

    public void Init(string name, BaseHero baseHero)
    {
        this.name = name;
        label_name.Text = name;
        this.baseHero = baseHero;
    }
    public void SetValue(Dice dice)
    {
        radioButton_csm.Checked = true;
        radioButton_exc.Checked = false;

        numeric_count.Value = dice.count;
        numeric_sides.Value = dice.sides;
        numeric_mod.Value = (decimal)(dice.modificator ?? 0f);
        numeric_exc.Value = (decimal)dice.expected;
        numeric_min.Value = (decimal)dice.min;
        numeric_max.Value = (decimal)dice.max;
    }

    public Dice GetValue()
    {
        float mod = (float)numeric_mod.Value;
        if (radioButton_csm.Checked)
        {
            return new Dice((int)numeric_count.Value, (int)numeric_sides.Value, mod != 0 ? mod : null);
        }
        else
        {
            float exp = (float)numeric_exc.Value;

            switch (baseHero?.rarity)
            {
                case 2:
                    exp *= 1.5f;
                    break;
                case 3:
                    exp *= 1.5f * 1.5f;
                    break;
                case 4:
                    exp *= 1.5f * 1.5f * 1.5f;
                    break;
                case 5:
                    exp *= 1.5f * 1.5f * (1.5f * 1.5f);
                    break;
            }

            if (name == "Health")
            {
                switch (baseHero?.mainStat)
                {
                    case General.EMainStat.strength:
                        exp *= 1.3f;
                        break;
                    case General.EMainStat.universal:
                        
                        break;
                    case General.EMainStat.agility:
                        exp *= 0.9f;
                        break;
                    case General.EMainStat.intelligence:
                        exp *= 0.8f;
                        break;
                    case null:
                        break;
                    default:
                        break;
                }
            } 
            if (name == "Damage")
            {
                switch (baseHero?.mainStat)
                {
                    case General.EMainStat.strength:
                        exp *= 0.95f;
                        break;
                    case General.EMainStat.universal:
                        break;
                    case General.EMainStat.agility:
                        exp *= 1.35f;
                        break;
                    case General.EMainStat.intelligence:
                        exp *= 1.1f;
                        break;
                    case null:
                        break;
                    default:
                        break;
                }
            }


            List<DCF.DiceCombination> list = DCF.FindDiceCombination((int)Math.Round(exp, 0), 0, 10, 10000);
            double target = 10;
            DCF.DiceCombination d = list.OrderBy(x => Math.Abs(x.CV - target)).First();

            return new Dice(d.Count, d.Sides, d.Mod != 0 ? d.Mod : null);
        }

    }

    private void radioButton_csm_CheckedChanged(object sender, EventArgs e)
    {
        radioButton_CheckedChanged();
    }
    private void radioButton_exc_CheckedChanged(object sender, EventArgs e)
    {
        radioButton_CheckedChanged();
    }

    private void radioButton_CheckedChanged()
    {
        bool b = radioButton_csm.Checked;
        numeric_count.Enabled = b;
        numeric_sides.Enabled = b;
        numeric_mod.Enabled = b;
        numeric_exc.Enabled = !b;
    }
}
