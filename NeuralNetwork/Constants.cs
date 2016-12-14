using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace NeuralNetwork
{
    public delegate void ShowProcessBar(float f);
    public delegate void AppendLogBox(String text);
    class Constants
    {
        public const Double MaxError = 1000000000.0;
        public const Double MinError = 0.0000001;

        public const int Input_Days = 40;
        public const int Output_Days = 1;
        public const int Training_Length = 300;//训练的样本数
        public const int Hidden_Layor_Count = 200;

        public const double Miu = 0.01;
        public const double Miu2 = 0.05;

        /// <summary>
        /// 数据的最大行数(天数)
        /// </summary>
        public const int UpboundRow = 900;
        public static ShowProcessBar ShowProcessBarAction;
        public static AppendLogBox AppendLogBoxAction;
        public const int TrainingCircles = 20;
        public static readonly String Execute_Directory;

        static Constants()
        {
            Execute_Directory = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar;
        }
    }

    public partial class Functions
    {
        private static void HttpDownloadFile(string url, string path)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                Stream fileStream = new FileStream(path, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    fileStream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                fileStream.Close();
                responseStream.Close();
            }
            catch (Exception ex)
            {
                Constants.AppendLogBoxAction(ex.Message);
            }
        }
    }

    public class ConvergenceException : Exception
    {
        public ConvergenceException(String message)
            : base(message)
        { }
    }

}
