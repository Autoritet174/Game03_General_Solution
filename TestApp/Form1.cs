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


    private static readonly char[] suffix = ['K', 'M', 'B', 'T', 'Q'];
    private static readonly int suffix_Length = suffix.Length;
    public static string NumberToShortString(float n)
    {
        // Обработка специальных случаев
        if (float.IsNaN(n) || float.IsInfinity(n)) return n.ToString();
        if (n == 0f) return "0";

        bool negative = n < 0f;
        float value = negative ? -n : n;

        if (value < 0.1f) {
            string result = value.ToString("0.00e0").Replace("e+", "e").Replace("E+", "e");
            return negative ? $"-{result}" : result;
        }

        if (value < 1f)
        {
            string result = value.ToString("0.###");
            return negative ? $"-{result}" : result;
        }

        string formatted = "";
        float originalValue = value; // Сохраняем для возможного научного формата
        int i;
        for (i = -1; i < suffix_Length; i++)
        {
            if (value < 10f)
            {
                formatted = value.ToString("0.##");
                break;
            }
            if (value < 100f)
            {
                formatted = value.ToString("0.#");
                break;
            }
            if (value < 1000f)
            {
                formatted = value.ToString("0");
                break;
            }

            value /= 1000f;
        }

        // Если число слишком большое для наших суффиксов
        if (formatted == "")
        {
            // Используем оригинальное значение для научной нотации
            formatted = originalValue.ToString("0.00e0").Replace("e+", "e").Replace("E+", "e");
        }
        else if (i > -1 && i < suffix_Length)
        {
            formatted += suffix[i];
        }
        
        return negative ? $"-{formatted}" : formatted;
    }
}
