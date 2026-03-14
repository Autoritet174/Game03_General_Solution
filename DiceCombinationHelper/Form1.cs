using System.Security.Policy;
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

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void numeric_diceCount_ValueChanged(object sender, EventArgs e)
    {
        CalcDice();
    }

    private void numeric_diceSize_ValueChanged(object sender, EventArgs e)
    {
        CalcDice();
    }

    private void numeric_diceModificator_ValueChanged(object sender, EventArgs e)
    {
        CalcDice();
    }

    private void CalcDice()
    {
        int diceCount = (int)numeric_diceCount.Value;
        int diceSize = (int)numeric_diceSize.Value;
        int modificator = (int)numeric_diceModificator.Value;
        //double mean = ((500L * (diceCount * (diceSize + 1))) + modificator) / 1000d; // Count * (Sides + 1) / 2 * 1000
        double mean = (diceCount * (diceSize + 1) / 2d) + (modificator / 1000d);
        double variance = diceCount * (Math.Pow(diceSize, 2) - 1) / 12.0;
        double stddev = Math.Sqrt(variance);
        double cv = stddev / mean * 100;
        double min = ((diceCount * 1000d) + modificator)/1000d;
        double max = ((1000d * (diceCount * diceSize)) + modificator)/1000d;

        textBox_diceResult.Text = $"""
            Mean: {mean:0.00}
            CV: {cv:0.00}%
            Min: {min:0.000}
            Max: {max:0.000}
            """;
    }

    private void numeric_diceCount_Enter(object sender, EventArgs e)
    {
        numeric_diceCount.Select(0, numeric_diceCount.Value.ToString().Length);
    }

    private void numeric_diceSize_Enter(object sender, EventArgs e)
    {
        numeric_diceSize.Select(0, numeric_diceSize.Value.ToString().Length);
    }

    private void numeric_diceModificator_Enter(object sender, EventArgs e)
    {
        numeric_diceModificator.Select(0, numeric_diceModificator.Value.ToString().Length);
    }

    private void numeric_diceCount_Click(object sender, EventArgs e)
    {
        numeric_diceCount.Select(0, numeric_diceCount.Value.ToString().Length);
    }
}
