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
            Constants.ShowProcessBarAction = invoke_ShowProcessBar;
            Constants.AppendLogBoxAction = invoke_AppendLogText;
            stockDictionary = new Dictionary<string, StockState>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<String> tagListBoxStocks = new List<string>();
            String StocksFile = Constants.Execute_Directory + "stocks.txt";
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
                stockState.loadHistoryData(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + Stocks[i] + ".csv");
                BP bpNetwork = new BP(Constants.Input_Days * 4, Constants.Hidden_Layor_Count, Constants.Output_Days * 4);
                stockState.neuralMatrix = bpNetwork;
                String filename = System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + Stocks[i] + ".data";
                stockState.loadHistoryData(filename);
                stockDictionary.Add(Stocks[i], stockState);
                task.process();
            }
        }

        private void btn_training_Click(object sender, EventArgs e)
        {
            foreach (StockState stockState in stockDictionary.Values)
            {
                Thread thrd = new Thread(stockState.training);
                thrd.Start();
            }
        }

        private void btn_legendretraining_Click(object sender, EventArgs e)
        {

        }


        private void lbStocks_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(((List<String> )lbStocks.Tag)[lbStocks.SelectedIndex]);
        }


        private void invoke_ShowProcessBar(float f)
        {
            ShowProcessBar func = new ShowProcessBar(operateProcessBar);
            object[] Params = { f };
            this.Invoke(func, Params);
        }
        private void operateProcessBar(float f)
        {
            progressBar1.Value = (int)(f * 100 + 0.5f);
        }

        private void updateAll()
        {
            foreach (StockState state in stockDictionary.Values)
            {
                Thread tread = new Thread(state.update);
                tread.Start();
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            updateAll();
        }

        private void appendTextBox(String text)
        {
            Console.WriteLine(text);
            tbLog.Text += "\n" + text;
        }
        private void invoke_AppendLogText(String text)
        {
            AppendLogBox func = new AppendLogBox(appendTextBox);
            object[] Params = { text };
            this.Invoke(func, Params);

        }

    }
}
