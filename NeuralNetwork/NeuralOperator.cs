using System;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
namespace NeuralNetwork
{

    public class StockState
    {
        public String code;
        public INeuralMatrix neuralMatrix;
        public double[,] historyData;
        public double[,] historyDataNormalize;
        public double[] predictData = null;
        //public double[] predictDataNormalize;
        /// <summary>
        /// 输入向量
        /// </summary>
        Vector input;
        double max = 0, min = 0;
        double coefficient;
        double offset;
        Vector Template;
        Task task;

        int Input_Days;
        int Output_Days;
        int Training_Length;//训练的样本数
        int Hidden_Layor_Count;

        public StockState(String code)
        {
            this.code = code;
            historyData = new double[Constants.UpboundRow, 4];
            historyDataNormalize = new double[Constants.UpboundRow, 4];
            //predictData = new double[Constants.Output_Days * 4];
            //predictDataNormalize = new double[Constants.Output_Days * 4];
            initial();
        }

        public void initial()
        {
            neuralMatrix = new BP(Constants.Input_Days * 4, Constants.Hidden_Layor_Count, Constants.Output_Days * 4);
            input = new Vector(Constants.Input_Days * 4);
            Template = new Vector(Constants.Output_Days * 4);
        }

        private void normalize()
        {
            double diff = max - min;
            double mid = min + (diff / 2.0);
            coefficient = diff * 1.0;
            offset = mid;
            normalize(coefficient, offset);
        }

        private void normalize(double coefficient, double offset)
        {
            for (int r = 0; r < Constants.UpboundRow; r++)
            {
                historyDataNormalize[r, 0] = (historyData[r, 0] - offset) / coefficient;
                historyDataNormalize[r, 1] = (historyData[r, 1] - offset) / coefficient;
                historyDataNormalize[r, 2] = (historyData[r, 2] - offset) / coefficient;
                historyDataNormalize[r, 3] = (historyData[r, 3] - offset) / coefficient;
            }
        }

        /// <summary>
        /// 从excel文件加载历史数据
        /// </summary>
        /// <param name="path"></param>
        public void loadHistoryData(string path)
        {
            if (!File.Exists(path))
            {
                Functions.downloadDataFile(code);
            }

            Excel._Application excel = null;
            Excel.Workbook workbook = null;
            try
            {
                excel = new Excel.Application();
                excel.Visible = false;
                Excel.Workbooks workbooks = excel.Workbooks;
                workbook = workbooks.Open(path);
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
                    if (tableRowNo >= Constants.UpboundRow)
                        break;
                }
                normalize();
            }
            catch (Exception ex)
            {
                Constants.AppendLogBoxAction(ex.Message);
                Console.ReadLine();
                //throw ex;
            }
            finally
            {
                try
                {
                    workbook.Close();
                }
                catch (Exception) { }
                try
                {
                    excel.Quit();
                }
                catch (Exception) { }
            }
        }

        public void loadNeuralMatrixData(String filename)
        {
            neuralMatrix.Load(filename);
        }

        public void save()
        {
            String filename = Constants.Execute_Directory + code + ".data";
            neuralMatrix.Save(filename);
        }

        public void predict()
        {
            input = new Vector(Constants.Input_Days * 4);
            for (int j = 0; j < Constants.Input_Days; j++)
            {
                input.item[j * 4 + 0] = historyDataNormalize[j, 0];
                input.item[j * 4 + 1] = historyDataNormalize[j, 1];
                input.item[j * 4 + 2] = historyDataNormalize[j, 2];
                input.item[j * 4 + 3] = historyDataNormalize[j, 3];
            }

            Vector output = neuralMatrix.Calculate(input);
            Console.Write("Result = ");
            for (int i = 0; i < output.UpperBound; i++)
                Constants.AppendLogBoxAction(String.Format("{0} ", (output.item[i] * coefficient + offset).ToString("F")));

            predictData = new double[40];
            for (int i = 0; i < 10 - Constants.Output_Days; i++)
            {
                predictData[i * 4 + 0] = historyData[10 - Constants.Output_Days - 1 - i, 0];
                predictData[i * 4 + 1] = historyData[10 - Constants.Output_Days - 1 - i, 1];
                predictData[i * 4 + 2] = historyData[10 - Constants.Output_Days - 1 - i, 2];
                predictData[i * 4 + 3] = historyData[10 - Constants.Output_Days - 1 - i, 3];
            }
            for (int i = 10 - Constants.Output_Days; i < 10; i++)
            {
                predictData[i * 4 + 0] = output.item[(i - (10 - Constants.Output_Days)) * 4 + 0] * coefficient + offset;
                predictData[i * 4 + 1] = output.item[(i - (10 - Constants.Output_Days)) * 4 + 1] * coefficient + offset;
                predictData[i * 4 + 2] = output.item[(i - (10 - Constants.Output_Days)) * 4 + 2] * coefficient + offset;
                predictData[i * 4 + 3] = output.item[(i - (10 - Constants.Output_Days)) * 4 + 3] * coefficient + offset;
            }

            for (int i = 0; i < 40; i++)
            {
                Constants.AppendLogBoxAction(predictData[i].ToString("F"));
            }
        }

        /*
         * BP神经网络的输入是天数*4、输出是天数*4 、隐含层的数量暂定400
         * 输入列分别开、高、低、收
         * 输入数组stockdata的行号由低到高表示日期由近及遥远过去
         * 输入向量随序列增加4个一组逐渐往过去延伸
         * 训练向量（输出）随序号增加逐渐往未来延伸
        */
        public void training()
        {
            Constants.AppendLogBoxAction(String.Format("Max = {0}; Min = {1}; Offset = {2}; Coeffi = {3}", max, min, offset, coefficient));
            task = Job.getInstance().addTask((Constants.TrainingCircles + 2) * (Constants.Training_Length - Constants.Output_Days + 1));
            for (int n = 0; n < 2; n++)
                for (int i = Constants.Output_Days; i <= Constants.Training_Length; i++)
                {
                    for (int j = 0; j < Constants.Input_Days; j++)
                    {
                        input.item[j * 4 + 0] = historyDataNormalize[i + j, 0];
                        input.item[j * 4 + 1] = historyDataNormalize[i + j, 1];
                        input.item[j * 4 + 2] = historyDataNormalize[i + j, 2];
                        input.item[j * 4 + 3] = historyDataNormalize[i + j, 3];
                    }
                    for (int d = 0; d < Constants.Output_Days; d++)
                    {
                        Template.item[d * 4 + 0] = historyDataNormalize[i - d - 1, 0];
                        Template.item[d * 4 + 1] = historyDataNormalize[i - d - 1, 1];
                        Template.item[d * 4 + 2] = historyDataNormalize[i - d - 1, 2];
                        Template.item[d * 4 + 3] = historyDataNormalize[i - d - 1, 3];
                    }

                    Constants.AppendLogBoxAction(String.Format("N = {0}, I = {1}", n, i));
                    try
                    {
                        neuralMatrix.Training(input, Template, 1);
                    }
                    catch (ConvergenceException ex)
                    {
                        Constants.AppendLogBoxAction(code + ex.Message);
                        task.finish();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Constants.AppendLogBoxAction(ex.Message);
                        task.finish();
                        return;
                    }
                    task.process();
                }

            for (int n = 0; n < Constants.TrainingCircles; n++)  //n是循环次数
                for (int i = Constants.Training_Length; i >= Constants.Output_Days; i--)
                {
                    for (int j = 0; j < Constants.Input_Days; j++)
                    {
                        input.item[j * 4 + 0] = historyDataNormalize[i + j, 0];
                        input.item[j * 4 + 1] = historyDataNormalize[i + j, 1];
                        input.item[j * 4 + 2] = historyDataNormalize[i + j, 2];
                        input.item[j * 4 + 3] = historyDataNormalize[i + j, 3];
                    }
                    for (int d = 0; d < Constants.Output_Days; d++)
                    {
                        Template.item[d * 4 + 0] = historyDataNormalize[i - d - 1, 0];
                        Template.item[d * 4 + 1] = historyDataNormalize[i - d - 1, 1];
                        Template.item[d * 4 + 2] = historyDataNormalize[i - d - 1, 2];
                        Template.item[d * 4 + 3] = historyDataNormalize[i - d - 1, 3];
                    }

                    Constants.AppendLogBoxAction(String.Format("N = {0}, I = {1}", n, i));
                    try
                    {
                        neuralMatrix.Training(input, Template, 8);
                    }
                    catch (ConvergenceException ex)
                    {
                        Constants.AppendLogBoxAction(code + ex.Message);
                        task.finish();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Constants.AppendLogBoxAction(ex.Message);
                        task.finish();
                        return;
                    }
                    task.process();
                }
            save();
        }

        public void update()
        {
            neuralMatrix = null;
            loadHistoryData("");
            //重新训练
            training();
            //保存
            save();
            //给出预测值
            predict();
        }
    }
}