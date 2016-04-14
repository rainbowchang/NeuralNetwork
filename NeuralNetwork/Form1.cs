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
        Job job;
        Dictionary<string, Object> stockDictionary;
        public Form1()
        {
            InitializeComponent();
            action = opserateProcessBar;
            stockDictionary = new Dictionary<string, Object>();
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
            //loadDataFile();
            bpNetwork = new BP(input_days * 4, hidden_layor_count, output_days * 4);
            initial();
        }

        protected void initial()
        {

        }

        private void btn_training_Click(object sender, EventArgs e)
        {
            //input_days = 200;
            //output_days = 3;
            //training_length = 300;//训练的样本数
            //hidden_layor_count = 400;

            Template = new Vector(output_days * 4);
            //bpNetwork = new BP(input_days * 4, hidden_layor_count, output_days * 4);

            Thread thread = new Thread(new ThreadStart(training));
            //thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
 //           bpNetwork.Save(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.data");
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            //bpNetwork.Load(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.data");
            //loadData(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.csv");
            //normalize(bpNetwork.coefficient, bpNetwork.offset);
        }

        private void btn_predict_Click(object sender, EventArgs e)
        {
            //input = new Vector(input_days * 4);
            //for (int j = 0; j < input_days; j++)
            //{
            //    input.item[j * 4 + 0] = stockData[j, 0];
            //    input.item[j * 4 + 1] = stockData[j, 1];
            //    input.item[j * 4 + 2] = stockData[j, 2];
            //    input.item[j * 4 + 3] = stockData[j, 3];
            //}

            //Vector output = bpNetwork.Calculate(input);
            //Console.Write("Result = ");
            //for (int i = 0; i < output.UpperBound; i++)
            //    Console.Write(String.Format("{0} ", (output.item[i] * bpNetwork.coefficient + bpNetwork.offset).ToString("F")));

            //double[] sample = new double[40];
            //for (int i = 0; i < 7; i++)
            //{
            //    sample[i * 4 + 0] = stockData[6 - i, 0];
            //    sample[i * 4 + 1] = stockData[6 - i, 1];
            //    sample[i * 4 + 2] = stockData[6 - i, 2];
            //    sample[i * 4 + 3] = stockData[6 - i, 3];
            //}
            //for (int i = 7; i < 10; i++)
            //{
            //    sample[i * 4 + 0] = output.item[(i - 7) * 4 + 0];
            //    sample[i * 4 + 1] = output.item[(i - 7) * 4 + 1];
            //    sample[i * 4 + 2] = output.item[(i - 7) * 4 + 2];
            //    sample[i * 4 + 3] = output.item[(i - 7) * 4 + 3];
            //}


            drawDailyK(sample);
        }

        private void btn_legendretraining_Click(object sender, EventArgs e)
        {
            //input_days = 150;
            //output_days = 2;
            //training_length = 100;//训练的样本数
            //hidden_layor_count = 25;
            //input = new Vector(input_days * 4);
            //Template = new Vector(output_days * 4);
            //bpNetwork = new LegendreMatrix(input_days * 4, hidden_layor_count, output_days * 4);

            //Thread thread = new Thread(new ThreadStart(training));
            ////thread.Priority = ThreadPriority.Highest;
            //thread.Start();
        }


        private void lbStocks_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(((List<String> )lbStocks.Tag)[lbStocks.SelectedIndex]);
        }

        public delegate void MyDelegate(float f);
        public MyDelegate action;
        private void opserateProcessBar(float f)
        {
            
        }
    }
}
