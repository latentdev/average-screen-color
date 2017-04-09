using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

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
        static public Color GetColorFromScreen(Point p, int x,int y)
        {
            Rectangle rect = new Rectangle(p, new Size(x, y));

            Bitmap map = CaptureFromScreen(rect);

            Color c = map.GetPixel(0, 0);

            map.Dispose();

            return c;
        }

        static public List<Color> horizontalColors(Point p, int x, int y, int points)
        {

            List<Color> colors = new List<Color>();
            int offset = x / points;

            Rectangle rect = new Rectangle(p, new Size(x, y));

            Bitmap map = CaptureFromScreen(rect);
            int in_x = 0;
            int in_y = y / 2;

            for (int i = 0; i < points; i++)
            {
                in_x = i * offset;
                colors.Add(map.GetPixel(in_x, in_y));
                Console.WriteLine("{0}: Red: {1}, Green: {2}, Blue: {3}", i, colors[i].R, colors[i].G, colors[i].B);
            }
            return colors;
        }


        static public Point[,] screenPoints(Point[,] points)
        {
            int xOffset = (int)System.Windows.SystemParameters.PrimaryScreenWidth / 4;
            int yOffset = (int)System.Windows.SystemParameters.PrimaryScreenHeight / 4;
            for (int y = 0; y < points.GetLength(1); y++)
            {
                for (int x = 0; x < points.GetLength(0); x++)
                {
                    points[x, y].X += xOffset * (x + 1);
                    points[x, y].Y += yOffset * (y + 1);
                    Console.WriteLine("X: " + points[x, y].X + " Y: " + points[x, y].Y);
                }
            }
            return points;
        }
        static public void averageColor(Point[,] points, SerialPort port)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Color c = Helper.GetColorFromScreen(points[x, y],2,2);
                    r += c.R;
                    b += c.B;
                    g += c.G;
                }

            }
            r /= 9;
            b /= 9;
            g /= 9;
            Console.WriteLine("Red: {0}, Green: {1}, Blue: {2}", r, g, b);
            port.Write("R");
            byte[] buffer = new byte[] { Convert.ToByte(r) };
            port.Write(buffer, 0, 1);
            buffer = new byte[] { Convert.ToByte(g) };
            port.Write(buffer, 0, 1);
            buffer = new byte[] { Convert.ToByte(b) };
            port.Write(buffer, 0, 1);
        }

        static public void colorBorder(Point[,] points, SerialPort port)
        {
            int width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int height = (int)System.Windows.SystemParameters.PrimaryScreenHeight;


        }
        static public void serialWrite(List<Color>[] horizontal, SerialPort port)
        {
            string letter;
            for (int i=0; i < horizontal.Count();i++)
            {
                switch(i)
                {
                    case 0:
                        letter = "T";
                        break;
                    case 1:
                        letter = "B";
                        break;
                    default:
                        letter = "T";
                        break;
                }
                
                port.Write(letter);
                byte[] buffer = new byte[] { Convert.ToByte(horizontal[i].Count()) };
                port.Write(buffer, 0, 1);
                for ( int m = 0;m < horizontal[i].Count(); m++)
                {
                    
                    buffer = new byte[] { Convert.ToByte(horizontal[i][m].R) };
                    port.Write(buffer, 0, 1);
                    buffer = new byte[] { Convert.ToByte(horizontal[i][m].G) };
                    port.Write(buffer, 0, 1);
                    buffer = new byte[] { Convert.ToByte(horizontal[i][m].B) };
                    port.Write(buffer, 0, 1);
                }
            }
        }
    }
}
