using System.Text;

namespace DiceCombinationHelper;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
        Calc();
    }

    private void numeric_mod_ValueChanged(object sender, EventArgs e)
    {
        Calc();
    }
    private void Calc()
    {
        StringBuilder stringBuilder = new();
        int val = (int)numeric_value.Value;
        int mod = (int)numeric_mod.Value;
        if (mod == 0 && numeric_modPerc.Value != 0)
        {
            mod = (int)(numeric_value.Value * numeric_modPerc.Value / 100m);
        }

        List<DiceCombinationFinder.DiceCombination> list = DiceCombinationFinder.FindDiceCombination(val, mod);
        foreach (DiceCombinationFinder.DiceCombination i in list)
        {
            //stringBuilder.AppendLine($"{i.Notation};    {i.CV}; {i.Mean}; {i.MeanError}; {i.CVError}; {i.TotalError}");
            _ = stringBuilder.AppendLine($"{i.Notation};    {i.CV:0.0};      {i.jsonb}");
        }
        textBox_Results.Text = stringBuilder.ToString();
    }
}
