using General.DTO;
using System.Xml.Linq;

namespace DataBaseEditor.Tables;

public partial class UCDice : UserControl
{
    public UCDice()
    {
        InitializeComponent();
        
    }

    public void SetName(string name) {
        label_name.Text = name;
    }
    public void SetValue(Dice dice)
    {
        radioButton_csm.Checked = false;
        radioButton_exc.Checked = true;

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
            List<DCF.DiceCombination> list = DCF.FindDiceCombination((double)numeric_exc.Value, 0, 10, 10000);
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
