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
        int input_days = 200;
        int output_days = 3;
        int training_length = 300;//训练的样本数
        int hidden_layor_count = 400;
        double coefficient;
        double offset;
        double max = 0, min = 0;

        /// <summary>
        /// 输入向量
        /// </summary>
        Vector input;// = new Vector(input_days * 4);

        /// <summary>
        /// 学习向量
        /// </summary>
        Vector Template;// = new Vector(output_days * 4);
        NeuralMatrix bpNetwork;// = new BP(input_days * 4, hidden_layor_count, output_days * 4);

        const int UpboundRow = 900;
        double[,] stockData = new double[UpboundRow, 4];

        public Form1()
        {
            InitializeComponent();
        }

        /*
         * BP神经网络的输入是天数*4、输出是天数*4 、隐含层的数量暂定400
         * 输入列分别开、高、低、收
         * 输入数组stockdata的行号有低到高表示日期由近及遥远过去
         * 输入向量随序列增加4个一组逐渐往过去延伸
         * 训练向量（输出）随序号增加逐渐往未来延伸
         */
        private void training()
        {
            try
            {
                loadData(@"E:\GitHub\NeuralNetwork\NeuralNetwork\bin\Debug\600036.csv");
            }
            catch (Exception)
            {
                return;
            }
            normalize();
            bpNetwork.coefficient = coefficient;
            bpNetwork.offset = offset;
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
                    try
                    {
                        bpNetwork.Training(input, Template, 5);
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
            input_days = 200;
            output_days = 3;
            training_length = 300;//训练的样本数
            hidden_layor_count = 400;
            input = new Vector(input_days * 4);
            Template = new Vector(output_days * 4);
            bpNetwork = new BP(input_days * 4, hidden_layor_count, output_days * 4);

            Thread thread = new Thread(new ThreadStart(training));
            //thread.Priority = ThreadPriority.Highest;
            thread.Start();
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
            coefficient = diff*10.0;
            offset = mid;
            normalize(coefficient, offset);
        }

        private void normalize(double coefficient, double offset)
        {
            for (int r = 0; r < UpboundRow; r++)
            {
                stockData[r, 0] = (stockData[r, 0] - offset) / coefficient;
                stockData[r, 1] = (stockData[r, 1] - offset) / coefficient;
                stockData[r, 2] = (stockData[r, 2] - offset) / coefficient;
                stockData[r, 3] = (stockData[r, 3] - offset) / coefficient;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            bpNetwork.Save(@"E:\GitHub\NeuralNetwork\NeuralNetwork\bin\Debug\600036.data");
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            bpNetwork.Load(@"E:\GitHub\NeuralNetwork\NeuralNetwork\bin\Debug\600036.data");
            loadData(@"E:\GitHub\NeuralNetwork\NeuralNetwork\bin\Debug\600036.csv");
            normalize(bpNetwork.coefficient, bpNetwork.offset);
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
                Console.Write(String.Format("{0} ", (output.item[i] * bpNetwork.coefficient + bpNetwork.offset).ToString("F")));
        }

        private void btn_legendretraining_Click(object sender, EventArgs e)
        {
            input_days = 150;
            output_days = 2;
            training_length = 100;//训练的样本数
            hidden_layor_count = 25;
            input = new Vector(input_days * 4);
            Template = new Vector(output_days * 4);
            bpNetwork = new LegendreMatrix(input_days * 4, hidden_layor_count, output_days * 4);

            Thread thread = new Thread(new ThreadStart(training));
            //thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
    }
}
