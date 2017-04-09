using System;
using System.Windows;
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
            SerialPort port = new SerialPort("COM4", 9600);
            port.Open();
            //Point[,] points = Helper.screenPoints(new Point[3,3]);
            List<Color>[] horizontals = new List<Color>[2];
            while (true)
            {
                horizontals[0] = Helper.horizontalColors(new Point(0, 0), (int)System.Windows.SystemParameters.PrimaryScreenWidth, 600, 5);
                horizontals[1] = Helper.horizontalColors(new Point(0, (int)System.Windows.SystemParameters.PrimaryScreenHeight-600), (int)System.Windows.SystemParameters.PrimaryScreenWidth, 600, 5);
                horizontals[0].Reverse();
                //Helper.averageColor(points, port);
                Helper.serialWrite(horizontals, port);
            }
            port.Close();
        }
    }
    
}
