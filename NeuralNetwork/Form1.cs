using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        const int input_days = 200;
        const int output_days = 3;
        const int training_length = 300;//训练的样本数
        const int hidden_layor_count = 400;
        /// <summary>
        /// 输入向量归一化时候的系数
        /// </summary>
        double coefficient = 0.0;
        /// <summary>
        /// 输入向量归一化时候的偏移量
        /// </summary>
        double offset = 0.0;

        double max = 0, min = 0;

        /// <summary>
        /// 输入向量
        /// </summary>
        Vector input = new Vector(input_days * 4);

        /// <summary>
        /// 学习向量
        /// </summary>
        Vector Template = new Vector(output_days * 4);
        BP bpNetwork = new BP(input_days * 4, hidden_layor_count, 4);

        const int UpboundRow = 600;
        double[,] stockData = new double[UpboundRow, 4];

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_predict_Click(object sender, EventArgs e)
        {

            for (int j = 0; j < input_days; j++)
            {
                input.item[j * 4 + 0] = stockData[j, 0];
                input.item[j * 4 + 1] = stockData[j, 1];
                input.item[j * 4 + 2] = stockData[j, 2];
                input.item[j * 4 + 3] = stockData[j, 3];
            }

            Vector output = bpNetwork.Calculate(input);
            Console.Write("Result = ");
            for (int i = 0; i < output.UpperBound; i++)
                Console.Write(String.Format("{0} ", output.item[i] * coefficient + offset));
            //Console.WriteLine(String.Format("Result = {0}, {1}, {2}, {3};", output.item[0] * coefficient + offset, output.item[1] * coefficient + offset, output.item[2] * coefficient + offset, output.item[3] * coefficient + offset));
        }

        private void training()
        {
            try
            {
                AnalysisTable(@"E:\GitHub\NeuralNetwork\NeuralNetwork\bin\Debug\table.csv");
            }
            catch (Exception)
            {
                return;
            }
            normalize();
            for (int n = 0; n <= 12; n++)  //n是循环次数
                for (int i = training_length; i >= output_days; i--)
                {
                    for (int j = 0; j < input_days; j++)
                    {
                        input.item[j * 4 + 0] = stockData[i + j, 0];
                        input.item[j * 4 + 1] = stockData[i + j, 1];
                        input.item[j * 4 + 2] = stockData[i + j, 2];
                        input.item[j * 4 + 3] = stockData[i + j, 3];
                    }
                    for (int d = 0; d < output_days; d++)
                    {
                        Template.item[d * 4 + 0] = stockData[i - d - 1, 0];
                        Template.item[d * 4 + 1] = stockData[i - d - 1, 1];
                        Template.item[d * 4 + 2] = stockData[i - d - 1, 2];
                        Template.item[d * 4 + 3] = stockData[i - d - 1, 3];
                    }

                    Console.WriteLine(String.Format("N = {0}, I = {1}", n, i));
                    Console.WriteLine(String.Format("Template = {0}, {1}, {2}, {3};", Template.item[0], Template.item[1], Template.item[2], Template.item[3]));
                    try
                    {
                        bpNetwork.Training(input, Template);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            Console.WriteLine("Training finish.");
            Console.ReadLine();
        }

        private void btn_training_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(training));
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        public void AnalysisTable(string path)
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
                for (int i = 2; i < 1000; i++)
                {
                    if (excel.Cells[i, 6].Value > 0.00001)
                    {
                        stockData[tableRowNo, 0] = excel.Cells[i, 2].Value;
                        if (max < stockData[tableRowNo, 0]) max = stockData[tableRowNo, 0];
                        if (min > stockData[tableRowNo, 0]) min = stockData[tableRowNo, 0];
                        stockData[tableRowNo, 1] = excel.Cells[i, 3].Value;
                        if (max < stockData[tableRowNo, 1]) max = stockData[tableRowNo, 1];
                        if (min > stockData[tableRowNo, 1]) min = stockData[tableRowNo, 1];
                        stockData[tableRowNo, 2] = excel.Cells[i, 4].Value;
                        if (max < stockData[tableRowNo, 2]) max = stockData[tableRowNo, 2];
                        if (min > stockData[tableRowNo, 2]) min = stockData[tableRowNo, 2];
                        stockData[tableRowNo, 3] = excel.Cells[i, 5].Value;
                        if (max < stockData[tableRowNo, 3]) max = stockData[tableRowNo, 3];
                        if (min > stockData[tableRowNo, 3]) min = stockData[tableRowNo, 3];
                        tableRowNo++;
                    }
                    if (tableRowNo >= UpboundRow)
                        break;
                }
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3};", stockData[0, 0], stockData[0, 1], stockData[0, 2], stockData[0, 3]));
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3};", stockData[2, 0], stockData[2, 1], stockData[2, 2], stockData[2, 3]));
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

        private void normalize()
        {
            double diff = max - min;
            double mid = min + (diff / 2.0);
            coefficient = diff;
            offset = mid;
            for (int r = 0; r < UpboundRow; r++)
            {
                stockData[r, 0] = (stockData[r, 0] - mid) / coefficient;
                stockData[r, 1] = (stockData[r, 1] - mid) / coefficient;
                stockData[r, 2] = (stockData[r, 2] - mid) / coefficient;
                stockData[r, 3] = (stockData[r, 3] - mid) / coefficient;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            bpNetwork.Save(@"C:\Users\Administrator\Desktop\nn.data");
        }
    }
}
