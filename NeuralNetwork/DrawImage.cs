using System.Drawing;
using System;


namespace NeuralNetwork
{
    public partial class Form1
    {
        private const int imageHeight = 200;
        private const int imageWidth = 500;
        private Image DailyKBackgroundImange = new Bitmap(imageWidth, imageHeight);
        private double DailyKMax = 1.0;
        private double dailyKMin = 0.0;

        private void drawDailyK(double[] data)
        {
            if (data == null || data.Length == 0 || data.Length % 4 != 0)
            {
                return;
            }
            Graphics grfx = Graphics.FromImage(DailyKBackgroundImange);
            grfx.Clear(Color.DarkGray);
            int d = data.Length / 4;
            for (int i = 0; i < d; i++)
            {
                double[] daily = new double[] { data[i * 4], data[i * 4 + 1], data[i * 4 + 2], data[i * 4 + 3] };
                drawDailyKPerDay(daily, false);
            }
        }

        private void setMaxMin(double[] data)
        {
            if (data == null || data.Length == 0)
                return;
            DailyKMax = data[0];
            dailyKMin = data[0];
            for (int i = 0; i < data.Length; i++)
            {
                if (DailyKMax < data[i])
                    DailyKMax = data[i];
                if (dailyKMin > data[i])
                    dailyKMin = data[i];
            }

            DailyKMax = Math.Ceiling(DailyKMax);
            dailyKMin = Math.Floor(dailyKMin);
        }

        private void drawDailyKPerDay(double[] data, Boolean predictFlag)
        {
            double open = data[0];
            double close = data[3];
            double max = data[1];
            double min = data[2];
            float maxWidth = 10.0f;
            float minWidth = 3.0f;

            Color currentColor;

            if (open > close)
                if (predictFlag)
                    currentColor = Color.Purple;
                else
                    currentColor = Color.Red;
            else
                if (predictFlag)
                    currentColor = Color.Blue;
                else
                    currentColor = Color.Green;

            Image image = new Bitmap(200, 100);
            Graphics itemgrfx = Graphics.FromImage(image);
            Pen pen = new Pen(currentColor, minWidth);
            itemgrfx.DrawLine(pen, 5, reletivePosition(DailyKMax, dailyKMin, imageHeight, max), 5, reletivePosition(DailyKMax, dailyKMin, imageHeight, min));
            pen = new Pen(currentColor, maxWidth);
            itemgrfx.DrawLine(pen, 5, reletivePosition(DailyKMax, dailyKMin, imageHeight, open), 5, reletivePosition(DailyKMax, dailyKMin, imageHeight, close));

            Graphics grfx = pic_box1.CreateGraphics();
            grfx.DrawImage(image, new Point(0, 0));

        }

        private float reletivePosition(double max, double min, double Height, double currentData)
        {
            double l = max - min;

            double currentPercent = (currentData - min) / l;

            double result = Height * (1 - currentPercent);

            return (float)result;
        }


    }

}

