namespace TestApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
        textBox1.Text = NumberToShortString((float)numericUpDown1.Value);
    }

    /*
     function NumberToShortString(n)
	if n < 100 then
		return string.sub(n, 0, 4)
	elseif n < 1000 then
		return string.sub(n, 0, 3)
        
	elseif n < 100000 then
		return string.sub(n/1000, 0, 4).."K"
	elseif n < 1000000 then
		return string.sub(n/1000, 0, 3).."K"
        
	elseif n < 100000000 then
		return string.sub(n/1000000, 0, 4).."M"
	elseif n < 1000000000 then
		return string.sub(n/1000000, 0, 3).."M"
        
	elseif n < 100000000000 then
		return string.sub(n/1000000000, 0, 4).."B"
	elseif n < 1000000000000 then
		return string.sub(n/1000000000, 0, 3).."B"
        
	else
        local result = string.gsub(string.format("%.2e", n), "e%+", "e")
        return result
	end
end
     */


    public static string NumberToShortString(double n)
    {
        if (n < 100)
        {
            return n.ToString().Substring(0, Math.Min(4, n.ToString().Length));
        }
        else if (n < 1000)
        {
            return n.ToString().Substring(0, Math.Min(3, n.ToString().Length));
        }
        else if (n < 100000)
        {
            return (n / 1000).ToString().Substring(0, Math.Min(4, (n / 1000).ToString().Length)) + "K";
        }
        else if (n < 1000000)
        {
            return (n / 1000).ToString().Substring(0, Math.Min(3, (n / 1000).ToString().Length)) + "K";
        }
        else if (n < 100000000)
        {
            return (n / 1000000).ToString().Substring(0, Math.Min(4, (n / 1000000).ToString().Length)) + "M";
        }
        else if (n < 1000000000)
        {
            return (n / 1000000).ToString().Substring(0, Math.Min(3, (n / 1000000).ToString().Length)) + "M";
        }
        else if (n < 100000000000)
        {
            return (n / 1000000000).ToString().Substring(0, Math.Min(4, (n / 1000000000).ToString().Length)) + "B";
        }
        else if (n < 1000000000000)
        {
            return (n / 1000000000).ToString().Substring(0, Math.Min(3, (n / 1000000000).ToString().Length)) + "B";
        }
        else
        {
            return n.ToString("0.00e0").Replace("e+", "e");
        }
    }
}
