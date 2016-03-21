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
        const int input_days = 100;
        const double xishu = 20.0;
        Vector input = new Vector(input_days * 4);
        Vector output = new Vector(4);
        BP bpNetwork = new BP(input_days * 4, 200, 4);

        double[,] stockData = new double[500, 4];

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

            output = bpNetwork.Calculate(input);
            Console.WriteLine(String.Format("Result = {0}, {1}, {2}, {3};", output.item[0] * 20, output.item[1] * 20, output.item[2] * 20, output.item[3] * 20));
            Console.ReadLine();
        }

        private void training()
        {
            AnalysisTable(@"C:\Users\Administrator\Desktop\table01.csv");
            for (int n = 0; n <= 4; n++)
                for (int i = 1; i <= 200; i++)
                {
                    for (int j = 0; j < input_days; j++)
                    {
                        input.item[j * 4 + 0] = stockData[i + j, 0];
                        input.item[j * 4 + 1] = stockData[i + j, 1];
                        input.item[j * 4 + 2] = stockData[i + j, 2];
                        input.item[j * 4 + 3] = stockData[i + j, 3];
                        //Console.WriteLine(String.Format("N = {0}, I = {1}, J = {2}", n, i, j));
                    }
                    output.item[0] = stockData[i - 1, 0];
                    output.item[1] = stockData[i - 1, 1];
                    output.item[2] = stockData[i - 1, 2];
                    output.item[3] = stockData[i - 1, 3];
                    Console.WriteLine(String.Format("N = {0}, I = {1}", n, i));
                    Console.WriteLine(String.Format("Template = {0}, {1}, {2}, {3};", output.item[0] * 20, output.item[1] * 20, output.item[2] * 20, output.item[3] * 20));
                    bpNetwork.Training(input, output);
                }
            Console.WriteLine("Training finish.");
            Console.ReadLine();
        }

        private void btn_training_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(training));
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
                    if (excel.Cells[i, 2].Value != 0.0)
                    {
                        stockData[tableRowNo, 0] = excel.Cells[i, 2].Value / xishu;
                        stockData[tableRowNo, 1] = excel.Cells[i, 3].Value / xishu;
                        stockData[tableRowNo, 2] = excel.Cells[i, 4].Value / xishu;
                        stockData[tableRowNo, 3] = excel.Cells[i, 5].Value / xishu;
                        tableRowNo++;
                    }
                    if (tableRowNo >= 500)
                        break;
                }
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3};", stockData[0, 0], stockData[0, 1], stockData[0, 2], stockData[0, 3]));
                Console.WriteLine(String.Format("{0}, {1}, {2}, {3};", stockData[2, 0], stockData[2, 1], stockData[2, 2], stockData[2, 3]));
                Console.ReadLine();
            }
            finally
            {
                excel.Quit();
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            bpNetwork.Save(@"C:\Users\Administrator\Desktop\nn.data");
        }
    }
}
