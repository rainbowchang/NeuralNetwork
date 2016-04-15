using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;


namespace NeuralNetwork
{
    public partial class Form1
    {
        public static void downloadDataFile(String code)
        {
            if (code.Length != 6)
                return;

            if (code.StartsWith("0"))
            {

            }
            else if (code.StartsWith("6"))
            {

            }
            else if (code.StartsWith("3"))
            {

            }
            else
            {

            }
        }

        public void updateAllStocksDataFile()
        {
            List<String> stocksList = (List<String>)lbStocks.Tag;
            foreach (string stockCode in stocksList)
            {
                downloadDataFile(stockCode);
            }
        }

        public static void HttpDownloadFile(string url, string path)
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
                Stream stream = new FileStream(path, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

}