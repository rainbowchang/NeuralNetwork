using System.Drawing;
using System;


namespace NeuralNetwork
{
    public partial class Form1
    {
        private const int imageHeight = 200;
        private const int imageWidth = 500;
        private Image DailyKBackgroundImange = new Bitmap(imageWidth, imageHeight);
        private double DailyKMax = 1.0;//K线中的最大数据
        private double dailyKMin = 0.0;//K线中的最小数据
        private float widthPerDay = 1.0f;//每K线所需要占用的画面宽度
        private float heightPerDay = imageHeight;//
        
        /// <summary>
        /// 按照输入画出K线
        /// </summary>
        /// <param name="data"></param>
        private void drawDailyK(double[] data)
        {
            if (data == null || data.Length == 0 || data.Length % 4 != 0)
            {
                return;
            }
            setMaxMin(data);
            Graphics grfx = Graphics.FromImage(DailyKBackgroundImange);
            grfx.Clear(Color.DarkGray);
            int d = data.Length / 4;
            widthPerDay = imageWidth / (d * 1.0f);
            Boolean predictFlag = false;
            for (int i = 0; i < d; i++)
            {
                if (i + 3 >= d)
                    predictFlag = true;
                else
                    predictFlag = false;
                drawDailyKPerDay(data, i, predictFlag);
            }
        }

        /// <summary>
        /// 设置K线数据里面的最大最小值
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// 画K线里面的每一天
        /// </summary>
        /// <param name="data"></param>
        /// <param name="day"></param>
        /// <param name="predictFlag"></param>
        private void drawDailyKPerDay(double[] data, int day, Boolean predictFlag)
        {
            double open = data[day * 4];
            double close = data[day * 4 + 3];
            double max = data[day * 4 + 1];
            double min = data[day * 4 + 2];
            float lineMaxWidth = 10.0f;
            float lineMinWidth = 3.0f;

            Color currentColor;

            if (open > close)
                if (predictFlag)
                    currentColor = Color.Pink;
                else
                    currentColor = Color.Red;
            else
                if (predictFlag)
                    currentColor = Color.GreenYellow;
                else
                    currentColor = Color.Green;

            Image image = new Bitmap(imageWidth, imageHeight);
            Graphics itemgrfx = Graphics.FromImage(image);
            Pen pen = new Pen(currentColor, lineMinWidth);
            float ancherXPosition = widthPerDay * day + widthPerDay / 2.0f;
            itemgrfx.DrawLine(pen, ancherXPosition, reletivePosition(DailyKMax, dailyKMin, imageHeight, max), ancherXPosition, reletivePosition(DailyKMax, dailyKMin, imageHeight, min));
            pen = new Pen(currentColor, lineMaxWidth);
            itemgrfx.DrawLine(pen, ancherXPosition, reletivePosition(DailyKMax, dailyKMin, imageHeight, open), ancherXPosition, reletivePosition(DailyKMax, dailyKMin, imageHeight, close));

            Graphics grfx = pic_box1.CreateGraphics();
            grfx.DrawImage(image, pic_box1.ClientRectangle);

        }

        /// <summary>
        /// 计算K线相对于画框的上下位置
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="Height"></param>
        /// <param name="currentData"></param>
        /// <returns></returns>
        private float reletivePosition(double max, double min, double Height, double currentData)
        {
            double l = max - min;

            double currentPercent = (currentData - min) / l;

            double result = Height * (1 - currentPercent);

            return (float)result;
        }
    }
}

