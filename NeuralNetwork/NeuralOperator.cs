using System;
using System.IO;
namespace NeuralNetwork
{
    public partial class Form1
    {
        int input_days = 200;
        int output_days = 3;
        int training_length = 300;//训练的样本数
        int hidden_layor_count = 400;


        /// <summary>
        /// 学习向量
        /// </summary>
        Vector Template;// = new Vector(output_days * 4);
        NeuralMatrix bpNetwork;// = new BP(input_days * 4, hidden_layor_count, output_days * 4);

        const int UpboundRow = 900;
        double[,] stockData = new double[UpboundRow, 4];


        /*
         * BP神经网络的输入是天数*4、输出是天数*4 、隐含层的数量暂定400
         * 输入列分别开、高、低、收
         * 输入数组stockdata的行号由低到高表示日期由近及遥远过去
         * 输入向量随序列增加4个一组逐渐往过去延伸
         * 训练向量（输出）随序号增加逐渐往未来延伸
         */
        private void training()
        {
            try
            {
                loadData(System.Windows.Forms.Application.StartupPath + Path.DirectorySeparatorChar + "600036.csv");
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

    }

}