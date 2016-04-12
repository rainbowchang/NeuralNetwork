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
using System.IO;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<String> tagListBoxStocks = new List<string>();
            String StocksFile = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "stocks.txt";
            StreamReader sr = File.OpenText(StocksFile);
            String str = "";
            while ((str = sr.ReadLine()) != null)
            {
                string[] ss = str.Split('=');
                ListViewItem item = new ListViewItem();
                item.Text = ss[0] + "  " + ss[1];
                item.Tag = ss[0];
                int index = lbStocks.Items.Add(ss[0] + " " + ss[1]);
                tagListBoxStocks.Add(ss[0]);
            }
            lbStocks.Tag = tagListBoxStocks;
            sr.Close();
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
            coefficient = diff * 10.0;
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
            bpNetwork.Save(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.data");
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            bpNetwork.Load(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.data");
            loadData(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.csv");
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

            drawDailyK
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


        private void lbStocks_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(((List<String> )lbStocks.Tag)[lbStocks.SelectedIndex]);
        }
    }
}
