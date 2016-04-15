using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    class Constants
    {
        public const Double MaxError = 1000000.0;
        public const Double MinError = 0.00000001;

        public const int Input_Days = 200;
        public const int Output_Days = 3;
        public const int Training_Length = 300;//训练的样本数
        public const int Hidden_Layor_Count = 400;
        
        public const double Miu = 0.05;

        /// <summary>
        /// 数据的最大行数(天数)
        /// </summary>
        public const int UpboundRow = 900;

    }
}
