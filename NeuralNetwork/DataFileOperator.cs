using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public partial class Functions
    {
        public static void downloadDataFile(String code)
        {
            String filename = Constants.Execute_Directory + code + ".csv";
            String url = "";
            String url_prefix = @"http://table.finance.yahoo.com/table.csv?s={0}.{1}";
            if (code.Length != 6)
                return;

            if (code.StartsWith("0"))
            {
                String suffix = "sz";
                url = String.Format(url_prefix, code, suffix);
            }
            else if (code.StartsWith("6"))
            {
                String suffix = "ss";
                url = String.Format(url_prefix, code, suffix);
            }
            else if (code.StartsWith("3"))
            {
                String suffix = "sz";
                url = String.Format(url_prefix, code, suffix);
            }
            else
            {

            }
            //下载
            HttpDownloadFile(url, filename);
        }

    }

    public partial class Form1
    {
        public void updateAllStocksDataFile()
        {
            List<String> stocksList = (List<String>)lbStocks.Tag;
            Task task = Job.getInstance().addTask(stocksList.Count);
            foreach (string stockCode in stocksList)
            {
                Functions.downloadDataFile(stockCode);
                task.process();
            }
        }



    }

}