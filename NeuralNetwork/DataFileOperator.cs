using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace NeuralNetwork
{
    public partial class Form1
    {
        public static void downloadDataFile(String code){
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

    public class StockState
    {
        public String code;
        public INeuralMatrix neuralMatrix;
        public double[,] historyData;
        public double[,] historyDataNormalize;
        public double[] predictData;
        public double[] predictDataNormalize;
        /// <summary>
        /// 输入向量
        /// </summary>
        Vector input;

        int input_days = 200;
        int output_days = 3;
        int training_length = 300;//训练的样本数
        int hidden_layor_count = 400;
        double max = 0, min = 0;
        double coefficient;
        double offset;
        const int UpboundRow = 900;
        Vector Template;

        public void initial()
        {
            input = new Vector(input_days * 4);
            Template = new Vector(output_days * 4);
        }

        private void normalize()
        {
            double diff = max - min;
            double mid = min + (diff / 2.0);
            coefficient = diff * 10.0;
            offset = mid;
            normalize(coefficient, offset);
        }

        private void normalize(double coefficient, double offset)
        {
            for (int r = 0; r < UpboundRow; r++)
            {
                historyDataNormalize[r, 0] = (historyData[r, 0] - offset) / coefficient;
                historyDataNormalize[r, 1] = (historyData[r, 1] - offset) / coefficient;
                historyDataNormalize[r, 2] = (historyData[r, 2] - offset) / coefficient;
                historyDataNormalize[r, 3] = (historyData[r, 3] - offset) / coefficient;
            }
        }

        private void loadData(string path)
        {
            Excel._Application excel = null;
            try
            {
                excel = new Excel.Application();
                excel.Visible = false;
                Excel.Workbooks workbooks = excel.Workbooks;
                Excel.Workbook workbook = workbooks.Open(path);
                Excel.Worksheet worksheet = workbook.Worksheets[1];
                int tableRowNo = 0;
                max = min = excel.Cells[2, 2].Value;//初始化！！！！！！
                for (int i = 2; i < 1000; i++)
                {
                    if (excel.Cells[i, 6].Value > 0.00001)
                    {
                        historyData[tableRowNo, 0] = excel.Cells[i, 2].Value;
                        if (max < historyData[tableRowNo, 0]) max = historyData[tableRowNo, 0];
                        if (min > historyData[tableRowNo, 0]) min = historyData[tableRowNo, 0];
                        historyData[tableRowNo, 1] = excel.Cells[i, 3].Value;
                        if (max < historyData[tableRowNo, 1]) max = historyData[tableRowNo, 1];
                        if (min > historyData[tableRowNo, 1]) min = historyData[tableRowNo, 1];
                        historyData[tableRowNo, 2] = excel.Cells[i, 4].Value;
                        if (max < historyData[tableRowNo, 2]) max = historyData[tableRowNo, 2];
                        if (min > historyData[tableRowNo, 2]) min = historyData[tableRowNo, 2];
                        historyData[tableRowNo, 3] = excel.Cells[i, 5].Value;
                        if (max < historyData[tableRowNo, 3]) max = historyData[tableRowNo, 3];
                        if (min > historyData[tableRowNo, 3]) min = historyData[tableRowNo, 3];
                        tableRowNo++;
                    }
                    if (tableRowNo >= UpboundRow)
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw ex;
            }
            finally
            {
                excel.Quit();
            }
        }

        public void save(String filename)
        {
            neuralMatrix.Save(filename);
        }

        public void training()
        {
            //neuralMatrix.Training(   ){

            //}
        }

        public void predict(){
            input = new Vector(input_days * 4);
            for (int j = 0; j < input_days; j++)
            {
                input.item[j * 4 + 0] = historyData[j, 0];
                input.item[j * 4 + 1] = historyData[j, 1];
                input.item[j * 4 + 2] = historyData[j, 2];
                input.item[j * 4 + 3] = historyData[j, 3];
            }

            Vector output = neuralMatrix.Calculate(input);
            Console.Write("Result = ");
            for (int i = 0; i < output.UpperBound; i++)
                Console.Write(String.Format("{0} ", (output.item[i] * coefficient + offset).ToString("F")));

            double[] sample = new double[40];
            for (int i = 0; i < 7; i++)
            {
                sample[i * 4 + 0] = historyData[6 - i, 0];
                sample[i * 4 + 1] = historyData[6 - i, 1];
                sample[i * 4 + 2] = historyData[6 - i, 2];
                sample[i * 4 + 3] = historyData[6 - i, 3];
            }
            for (int i = 7; i < 10; i++)
            {
                sample[i * 4 + 0] = output.item[(i - 7) * 4 + 0];
                sample[i * 4 + 1] = output.item[(i - 7) * 4 + 1];
                sample[i * 4 + 2] = output.item[(i - 7) * 4 + 2];
                sample[i * 4 + 3] = output.item[(i - 7) * 4 + 3];
            }
        }
    }
}