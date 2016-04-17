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
        /// <summary>
        /// 神经网络实例集合
        /// </summary>
        private Dictionary<string, StockState> stockDictionary;

        public Form1()
        {
            InitializeComponent();
            Constants.action = operateProcessBar;
            stockDictionary = new Dictionary<string, StockState>();
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
                tagListBoxStocks.Add(ss[0].Trim());
            }
            lbStocks.Tag = tagListBoxStocks;
            sr.Close();
            Thread thread = new Thread(initial);
            thread.Start();
        }

        protected void initial()
        {
            List<String> Stocks = (List<String>)lbStocks.Tag;
            Task task = Job.getInstance().addTask(Stocks.Count);
            for (int i = 0; i < Stocks.Count; i++)
            {
                StockState stockState = new StockState(Stocks[i]);
                stockState.loadHistoryData(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + Stocks[i] + ".cvs");
                BP bpNetwork = new BP(Constants.Input_Days * 4, Constants.Hidden_Layor_Count, Constants.Output_Days * 4);
                stockState.neuralMatrix = bpNetwork;
                String filename = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + Stocks[i] + ".data";
                bpNetwork.Load(filename);
                stockDictionary.Add(Stocks[i], stockState);
                task.process();
            }
        }

        private void btn_training_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(training));
            //thread.Start();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {

        }

        private void btn_load_Click(object sender, EventArgs e)
        {

        }

        private void btn_predict_Click(object sender, EventArgs e)
        {
            //drawDailyK(sample);
        }

        private void btn_legendretraining_Click(object sender, EventArgs e)
        {

        }


        private void lbStocks_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(((List<String> )lbStocks.Tag)[lbStocks.SelectedIndex]);
        }

        private void operateProcessBar(float f)
        {

        }

        private void updateAll()
        {
            foreach (StockState state in stockDictionary.Values)
            {
                Thread tread = new Thread(state.update);
                tread.Start();
            }
        }
    }
}
