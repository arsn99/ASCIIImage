using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
namespace ASCIIImage
{
    class Program
    {
        private const double WIDTH_OFFSET = 1.5;
        private const int MAXWIDTH = 600;

        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
            };
            Console.WriteLine("Press enter...");
            while (true)
            {
                Console.ReadLine();
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    continue;

                Console.Clear();

                var bitmap = new Bitmap(openFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayScale();

                var converter = new BitmapToASCIIConverter(bitmap);
                var rows = converter.Convert();
                var rowsNegative = converter.ConvertNegative();
                foreach (var row in rows)
                    Console.WriteLine(row);
                File.WriteAllLines("image.txt", rowsNegative.Select(r => new string(r)));

                Console.SetCursorPosition(0, 0);
            }
        }
        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            
            var newHeight = bitmap.Height / WIDTH_OFFSET * MAXWIDTH / bitmap.Width;
            if (bitmap.Width > MAXWIDTH || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(MAXWIDTH, (int)newHeight));
            return bitmap;
        }
    }
}
