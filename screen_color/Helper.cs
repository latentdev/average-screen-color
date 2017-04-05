using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screen_color
{
    static class Helper
    {
        static public Bitmap CaptureFromScreen(Rectangle rect)
        {
            Bitmap bmpScreenCapture = null;

            if (rect == Rectangle.Empty)//capture the whole screen
            {
                bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);
            }
            else // just the rect
            {
                bmpScreenCapture = new Bitmap(rect.Width, rect.Height);
            }

            Graphics p = Graphics.FromImage(bmpScreenCapture);

            if (rect == Rectangle.Empty)
            { // captuer the whole screen
                p.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                         Screen.PrimaryScreen.Bounds.Y,
                                         0, 0,
                                         bmpScreenCapture.Size,
                                         CopyPixelOperation.SourceCopy);

            }
            
            else // cut a spacific rectangle
            {
                try
                {
                    p.CopyFromScreen(rect.X,
                         rect.Y,
                         0, 0,
                         rect.Size,
                         CopyPixelOperation.SourceCopy);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            return bmpScreenCapture;
        }
        static public Color GetColorFromScreen(Point p)
        {
            Rectangle rect = new Rectangle(p, new Size(2, 2));

            Bitmap map = CaptureFromScreen(rect);

            Color c = map.GetPixel(0, 0);

            map.Dispose();

            return c;
        }
    }
}
