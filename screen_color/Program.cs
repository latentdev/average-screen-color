using System;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace screen_color
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort port = new SerialPort("COM3", 9600);
            port.Open();
            Point[,] points = new Point[3,3];
            int xOffset = 2560 / 4;
            int yOffset = 1440 / 4;
            for (int y=0;y<3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    points[x,y].X += xOffset * (x + 1);
                    points[x,y].Y += yOffset * (y + 1);
                    Console.WriteLine("X: " + points[x,y].X + " Y: " + points[x,y].Y);
                }
            }

            int r = 0;
            int b = 0;
            int g = 0;

            while (true)
            {
                r = 0;
                b = 0;
                g = 0;
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        Color c = Helper.GetColorFromScreen(points[x,y]);
                        r += c.R;
                        b += c.B;
                        g += c.G;
                    }
                    
                }
                r /= 9;
                b /= 9;
                g /= 9;
                Console.WriteLine("Red: {0}, Green: {1}, Blue: {2}",r,g,b);
                port.Write("R");
                byte[] buffer = new byte[] { Convert.ToByte(r) };
                port.Write(buffer, 0, 1);
                buffer = new byte[] { Convert.ToByte(g) };
                port.Write(buffer, 0, 1);
                buffer = new byte[] { Convert.ToByte(b) };
                port.Write(buffer, 0, 1);
            }
            port.Close();
        }
    }
    
}
