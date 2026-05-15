using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataBaseEditor.Tables;

public partial class BaseHeroesForm : Form
{
    public BaseHeroesForm()
    {
        InitializeComponent();
    }

    private void BaseHeroesForm_Load(object sender, EventArgs e)
    {
        ucDiceHealth.SetName("Health");
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var d = ucDiceHealth.GetValue();
        button1.Text = d.ToStr();
    }
}
