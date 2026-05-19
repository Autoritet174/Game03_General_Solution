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

        numeric_count.Value = dice.Count;
        numeric_sides.Value = dice.Sides;
        numeric_mod.Value = (decimal)(dice.Modificator ?? 0f);
        numeric_exc.Value = (decimal)dice.Expected;
        numeric_min.Value = (decimal)dice.Min;
        numeric_max.Value = (decimal)dice.Max;
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

            switch (baseHero?.Rarity)
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
                switch (baseHero?.MainStat)
                {
                    case General.EMainStat.Strength:
                        exp *= 1.3f;
                        break;
                    case General.EMainStat.Universal:
                        
                        break;
                    case General.EMainStat.Agility:
                        exp *= 0.9f;
                        break;
                    case General.EMainStat.Intelligence:
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
                switch (baseHero?.MainStat)
                {
                    case General.EMainStat.Strength:
                        exp *= 0.95f;
                        break;
                    case General.EMainStat.Universal:
                        break;
                    case General.EMainStat.Agility:
                        exp *= 1.35f;
                        break;
                    case General.EMainStat.Intelligence:
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
