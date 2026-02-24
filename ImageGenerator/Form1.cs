using System.Drawing.Imaging;

namespace ImageGenerator;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        MakeEdgesTransparent(
            @"C:\UnityProjects\Game03_Git\Client_Game03\Assets\GameData\AddressableAssets\Images\UI\CollectionElementSelected_Original.png",
            @"C:\UnityProjects\Game03_Git\Client_Game03\Assets\GameData\AddressableAssets\Images\UI\CollectionElementSelected.png");
        Close();
    }

    private static int A(int a)
    {
        return a < 0 ? 0 : a > 255 ? 255 : a;
    }

    public static void MakeEdgesTransparent(string inputPath, string outputPath)
    {
        int dist = 35;
        // Проверяем, что изображение 256x256
        using var image = Image.FromFile(inputPath);
        if (image.Width != 256 || image.Height != 256)
        {
            throw new ArgumentException("Изображение должно быть 256x256");
        }

        // Создаем новый битмап с альфа-каналом
        using var bitmap1 = new Bitmap(256, 256, PixelFormat.Format32bppArgb);
        // Копируем исходное изображение
        using (var g = Graphics.FromImage(bitmap1))
        {
            g.DrawImage(image, 0, 0, 256, 256);
        }

        // Левая сторона (x: 0-27)
        for (int x = 0; x < dist; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                Color c = bitmap1.GetPixel(x, y);
                // Прозрачность = (x / 28) * 255, но инвертировано
                int alpha = c.A * x / dist;
                alpha = A(alpha);
                bitmap1.SetPixel(x, y, Color.FromArgb(alpha, c));
            }
        }

        // Правая сторона (x: 228-255)
        for (int x = 256 - dist; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                Color c = bitmap1.GetPixel(x, y);
                int distanceFromRight = 255 - x;
                int alpha = c.A * (256 - x) / dist;
                alpha = A(alpha);
                bitmap1.SetPixel(x, y, Color.FromArgb(alpha, c));
            }
        }

        // Верхняя сторона (y: 0-27) - пропускаем уже обработанные углы
        for (int y = 0; y < dist; y++)
        {
            for (int x = 0; x < 256; x++)
            {
                Color c = bitmap1.GetPixel(x, y);
                int alpha = c.A * y / dist;
                alpha = A(alpha);
                bitmap1.SetPixel(x, y, Color.FromArgb(alpha, c));
            }
        }

        // Нижняя сторона (y: 228-255)
        for (int y = 256 - dist; y < 256; y++)
        {
            for (int x = 0; x < 256; x++)
            {
                Color c = bitmap1.GetPixel(x, y);
                int alpha = c.A * (256 - y) / dist;
                alpha = A(alpha);
                bitmap1.SetPixel(x, y, Color.FromArgb(alpha, c));
            }
        }

        //double range = Math.Sqrt(Math.Pow(100, 2) * 2);
        //double rangeMax = Math.Sqrt(Math.Pow(128, 2) * 2);
        //// левый верхний
        //for (int x = 1; x <= 256; x++)
        //{
        //    for (int y = 1; y <= 256; y++)
        //    {
        //        double r = Math.Sqrt(Math.Pow(Math.Abs(128 - x), 2) + Math.Pow(Math.Abs(128 - y), 2));
        //        if (r < range) {
        //            continue;
        //        }

        //        Color c = bitmap1.GetPixel(x-1, y-1);
        //        int alpha = (int)(c.A * range / rangeMax);
        //        alpha = A(alpha);
        //        bitmap1.SetPixel(x-1, y-1, Color.FromArgb(alpha, c));
        //    }
        //}

        // Сохраняем как PNG
        bitmap1.Save(outputPath, ImageFormat.Png);
    }
}
