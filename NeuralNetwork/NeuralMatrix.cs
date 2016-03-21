using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NeuralNetwork
{
    class INeuralMatrix
    {


    }


    class BP : INeuralMatrix
    {
        /// <summary>
        /// 输入向量
        /// </summary>
        Vector Input_Layer_Vector;
        /// <summary>
        /// 隐含层输出向量
        /// </summary>
        Vector Hidden_Layer_Vector;
        /// <summary>
        /// 输出向量
        /// </summary>
        Vector Output_Layer_Vector;
        /// <summary>
        /// 预期结果向量
        /// </summary>
        Vector Template_Vector;
        /// <summary>
        /// 隐含层偏移量
        /// </summary>
        Vector Hidden_Offset_Vector;
        /// <summary>
        /// 隐含层偏移量调整量
        /// </summary>
        Vector Hidden_Offset_Chang_Vector;
        /// <summary>
        /// 输出层偏移量
        /// </summary>            
        Vector Output_Offset_Vector;
        /// <summary>
        /// 输出层偏移量调整量
        /// </summary>
        Vector Output_Offset_Change_Vector;

        /// <summary>
        /// 输入层单元数量
        /// </summary>
        int Input_Layer_Count;
        /// <summary>
        /// 隐含层单元数量
        /// </summary>
        int Hidden_Layer_Count;
        /// <summary>
        /// 输出层单元数量
        /// </summary>
        int Output_Layer_Count;

        /// <summary>
        /// 输入层到隐含层的系数矩阵
        /// </summary>
        Matrix Input_Hiddene_Coefficient_Matrix;
        /// <summary>
        /// 隐含层到输出层的系数矩阵
        /// </summary>
        Matrix Hidden_Output_Coefficient_Matrix;
        /// <summary>
        /// 输入层到隐含层的系数偏移矩阵
        /// </summary>
        Matrix Input_Hidden_Coefficient_Change_Matrix;
        /// <summary>
        /// 输入层到隐含层的Delta系数矩阵
        /// </summary>
        Matrix Input_Hidden_Coefficient_Delta_Matrix;
        /// <summary>
        /// 输入层到隐含层的系数矩阵
        /// </summary>
        Matrix Hidden_Output_Coefficient_Change_Matrix;
        /// <summary>
        /// 输入层到隐含层的Delta系数矩阵
        /// </summary>
        Matrix Hidden_Output_Coefficient_Delta_Matrix;
        double Miu;
        double Delta;

        INeuron Function1 = new Logsig();
        INeuron Function2 = new Pureline();

        /// <summary>
        /// 私有构造函数，适合于从文件读取连接系数的时候从Load函数内部调用。
        /// </summary>
        private BP()
        {
            Miu = 0.01;
            Delta = 0.0;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inputLayerCount"></param>
        /// <param name="hiddenLayerNumber"></param>
        /// <param name="outputLayerCount"></param>
        public BP(int inputLayerCount, int hiddenLayerNumber, int outputLayerCount)
        {
            this.Input_Layer_Count = inputLayerCount;
            this.Hidden_Layer_Count = hiddenLayerNumber;
            this.Output_Layer_Count = outputLayerCount;

            Input_Layer_Vector = new Vector(Input_Layer_Count);
            Hidden_Layer_Vector = new Vector(Hidden_Layer_Count);
            Output_Layer_Vector = new Vector(Output_Layer_Count);
            Template_Vector = new Vector(Output_Layer_Count);
            Hidden_Offset_Vector = new Vector(Hidden_Layer_Count);
            Hidden_Offset_Chang_Vector = new Vector(Hidden_Layer_Count);
            Output_Offset_Vector = new Vector(Output_Layer_Count);
            Output_Offset_Change_Vector = new Vector(Output_Layer_Count);

            Input_Hiddene_Coefficient_Matrix = new Matrix(Hidden_Layer_Count, Input_Layer_Count);
            Hidden_Output_Coefficient_Matrix = new Matrix(Output_Layer_Count, Hidden_Layer_Count);
            Input_Hidden_Coefficient_Change_Matrix = new Matrix(Hidden_Layer_Count, Input_Layer_Count);
            Input_Hidden_Coefficient_Delta_Matrix = new Matrix(Hidden_Layer_Count, Input_Layer_Count);
            Hidden_Output_Coefficient_Change_Matrix = new Matrix(Output_Layer_Count, Hidden_Layer_Count);
            Hidden_Output_Coefficient_Delta_Matrix = new Matrix(Output_Layer_Count, Hidden_Layer_Count);
            Miu = 0.1;
            Delta = 0.0;

            InitialMatrix();
        }

        /// <summary>
        /// 初始化网络参数
        /// </summary>
        private void InitialMatrix()
        {
            Random rnd = new Random(DateTime.Now.Second);

            for (int i = 0; i < Hidden_Layer_Count; i++)
                for (int j = 0; j < Input_Layer_Count; j++)
                    Input_Hiddene_Coefficient_Matrix.item[i, j] = rnd.NextDouble() - 0.5;

            for (int k = 0; k < Output_Layer_Count; k++)
                for (int i = 0; i < Hidden_Layer_Count; i++)
                    Hidden_Output_Coefficient_Matrix.item[k, i] = rnd.NextDouble() - 0.5;

            for (int i = 0; i < Hidden_Layer_Count; i++)
                Hidden_Offset_Vector.item[i] = rnd.NextDouble() - 0.5;

            for (int k = 0; k < Output_Layer_Count; k++)
                Output_Offset_Vector.item[k] = rnd.NextDouble() - 0.5;
        }

        /// <summary>
        /// 根据输入向量，计算出输出向量。
        /// </summary>
        public void Calculate()
        {
            Hidden_Layer_Vector = Input_Hiddene_Coefficient_Matrix * Input_Layer_Vector;
            Hidden_Layer_Vector = Hidden_Layer_Vector + Hidden_Offset_Vector;
            for (int i = 0; i < Hidden_Layer_Count; i++)
            {
                Hidden_Layer_Vector.item[i] = Function1.NeuronFunction(Hidden_Layer_Vector.item[i]);
            }

            Output_Layer_Vector = Hidden_Output_Coefficient_Matrix * Hidden_Layer_Vector;
            Output_Layer_Vector = Output_Layer_Vector + Output_Offset_Vector;
            for (int k = 0; k < Output_Layer_Count; k++)
            {
                Output_Layer_Vector.item[k] = Function2.NeuronFunction(Output_Layer_Vector.item[k]);
                Console.WriteLine("{0}={1}  ", k, Output_Layer_Vector.item[k].ToString());
            }
        }

        public Vector Calculate(Vector vectorInput)
        {
            Input_Layer_Vector = vectorInput;
            Calculate();
            return Output_Layer_Vector;
        }

        /// <summary>
        /// 网络训练函数。
        /// </summary>
        /// <param name="Input_Vector"></param>
        /// <param name="Template_Vector"></param>
        public void Training(Vector Input_Vector, Vector Template_Vector)
        {
            this.Input_Layer_Vector = Input_Vector;
            this.Template_Vector = Template_Vector;
            for (int a = 0; a < 6; a++)
            {
                Calculate();
                CalculateError();
                CalculateDelta_Hidden_Output_Coefficient();
                CalculateDelta_Input_Hidden_Coefficient();

                Hidden_Output_Coefficient_Matrix += Hidden_Output_Coefficient_Change_Matrix;
                Input_Hiddene_Coefficient_Matrix += Input_Hidden_Coefficient_Change_Matrix;
                Console.Write("Mark    line 203");
                Hidden_Offset_Vector += Hidden_Offset_Chang_Vector;
                Output_Offset_Vector += Output_Offset_Change_Vector;
            }
        }

        /// <summary>
        /// 计算输出误差Delta。
        /// </summary>
        /// <returns></returns>
        private double CalculateError()
        {
            double e = 0.0;

            for (int k = 0; k < Output_Layer_Count; k++)
            {
                double delta = Output_Layer_Vector.item[k] - Template_Vector.item[k];
                e += delta * delta;
            }

            Delta = e / 2.0;
            Console.WriteLine(String.Format("Delta = {0}", Delta.ToString()));
            return Delta;
        }

        /// <summary>
        /// 计算隐含层-输出层系数、偏移量的调整量
        /// </summary>
        private void CalculateDelta_Hidden_Output_Coefficient()
        {
            Console.WriteLine("CalculateDelta_Hidden_Output_Coefficient");
            for (int k = 0; k < Output_Layer_Count; k++)
            {
                double delta = Function2.NeuronFunctionDerivative(Output_Layer_Vector.item[k]) * (Template_Vector.item[k] - Output_Layer_Vector.item[k]);

                for (int i = 0; i < Hidden_Layer_Count; i++)
                {
                    Hidden_Output_Coefficient_Delta_Matrix.item[k, i] = delta;
                    Hidden_Output_Coefficient_Change_Matrix.item[k, i] = delta * Miu * Hidden_Layer_Vector.item[i];
                }

                Output_Offset_Change_Vector.item[k] = delta * Miu;
            }
        }


        /// <summary>
        /// 计算输入层-隐含层系数、偏移量的调整量
        /// </summary>
        private void CalculateDelta_Input_Hidden_Coefficient()
        {
            Console.WriteLine("CalculateDelta_Input_Hidden_Coefficient");
            for (int i = 0; i < Hidden_Layer_Count; i++)
            {
                double delta = 0;
                for (int k = 0; k < Output_Layer_Count; k++)
                {
                    delta += Hidden_Output_Coefficient_Delta_Matrix.item[k, i] * Hidden_Output_Coefficient_Matrix.item[k, i];
                }
                delta *= Function1.NeuronFunctionDerivative(Hidden_Layer_Vector.item[i]);
                for (int j = 0; j < Input_Layer_Count; j++)
                {
                    Input_Hidden_Coefficient_Delta_Matrix.item[i, j] = delta;
                    Input_Hidden_Coefficient_Change_Matrix.item[i, j] = Miu * delta * Input_Layer_Vector.item[j];
                }
                Hidden_Offset_Chang_Vector.item[i] = delta * Miu;
            }
        }

        /// <summary>
        /// 从文件中加载一个BP网络
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public static BP Load(String Filename)
        {
            FileStream fs = new FileStream(Filename, FileMode.Open);
            BinaryReader r = new BinaryReader(fs);

            BP bp = new BP();

            bp.Input_Layer_Count = r.ReadInt32();
            bp.Hidden_Layer_Count = r.ReadInt32();
            bp.Output_Layer_Count = r.ReadInt32();

            bp.Input_Layer_Vector = new Vector(bp.Input_Layer_Count);
            bp.Hidden_Layer_Vector = new Vector(bp.Hidden_Layer_Count);
            bp.Output_Layer_Vector = new Vector(bp.Output_Layer_Count);
            bp.Template_Vector = new Vector(bp.Output_Layer_Count);
            bp.Hidden_Offset_Vector = new Vector(bp.Hidden_Layer_Count);
            bp.Hidden_Offset_Chang_Vector = new Vector(bp.Hidden_Layer_Count);
            bp.Output_Offset_Vector = new Vector(bp.Output_Layer_Count);
            bp.Output_Offset_Change_Vector = new Vector(bp.Output_Layer_Count);

            bp.Input_Hiddene_Coefficient_Matrix = new Matrix(bp.Hidden_Layer_Count, bp.Input_Layer_Count);
            bp.Hidden_Output_Coefficient_Matrix = new Matrix(bp.Output_Layer_Count, bp.Hidden_Layer_Count);
            bp.Input_Hidden_Coefficient_Change_Matrix = new Matrix(bp.Hidden_Layer_Count, bp.Input_Layer_Count);
            bp.Input_Hidden_Coefficient_Delta_Matrix = new Matrix(bp.Hidden_Layer_Count, bp.Input_Layer_Count);
            bp.Hidden_Output_Coefficient_Change_Matrix = new Matrix(bp.Output_Layer_Count, bp.Hidden_Layer_Count);
            bp.Hidden_Output_Coefficient_Delta_Matrix = new Matrix(bp.Output_Layer_Count, bp.Hidden_Layer_Count);

            for (int i = 0; i < bp.Hidden_Layer_Count; i++)
                for (int j = 0; j < bp.Input_Layer_Count; j++)
                    bp.Input_Hiddene_Coefficient_Matrix.item[i, j] = r.ReadDouble();

            for (int i = 0; i < bp.Hidden_Layer_Count; i++)
                bp.Hidden_Offset_Vector.item[i] = r.ReadDouble();

            for (int k = 0; k < bp.Output_Layer_Count; k++)
                for (int i = 0; i < bp.Hidden_Layer_Count; i++)
                    bp.Hidden_Output_Coefficient_Matrix.item[k, i] = r.ReadDouble();

            for (int k = 0; k < bp.Output_Layer_Count; k++)
                bp.Output_Offset_Vector.item[k] = r.ReadDouble();
            r.Close();
            fs.Close();
            return bp;
        }

        /// <summary>
        /// 将连接系数和偏移函数保存到文件。
        /// </summary>
        /// <param name="Filename"></param>
        public void Save(String Filename)
        {
            FileStream fs = new FileStream(Filename, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write((int)Input_Layer_Count);
            w.Write((int)Hidden_Layer_Count);
            w.Write((int)Output_Layer_Count);

            for (int i = 0; i < Hidden_Layer_Count; i++)
                for (int j = 0; j < Input_Layer_Count; j++)
                {
                    w.Write(Input_Hiddene_Coefficient_Matrix.item[i, j]);
                }


            for (int i = 0; i < Hidden_Layer_Count; i++)
            {
                w.Write(Hidden_Offset_Vector.item[i]);
            }

            for (int k = 0; k < Output_Layer_Count; k++)
                for (int i = 0; i < Hidden_Layer_Count; i++)
                {
                    w.Write(Hidden_Output_Coefficient_Matrix.item[k, i]);
                }

            for (int k = 0; k < Output_Layer_Count; k++)
            {
                w.Write(Output_Offset_Vector.item[k]);
            }

            w.Close();
            fs.Close();
        }
    }
}
