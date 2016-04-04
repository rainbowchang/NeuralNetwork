using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NeuralNetwork
{
    class LegendreMatrix : NeuralMatrix
    {

        public LegendreMatrix()
        {
            Miu = 0.1;
            Delta = 0.0;
        }

        public LegendreMatrix(int inputLayerCount, int hiddenLayerNumber, int outputLayerCount)
        {
            this.Input_Layer_Count = inputLayerCount;
            this.Hidden_Layer_Count = hiddenLayerNumber;
            this.Output_Layer_Count = outputLayerCount;

            FirstNeuronVector = new INeuron[hiddenLayerNumber];
            SecondNeuronVector = new INeuron[outputLayerCount];

            Input_Layer_Vector = new Vector(Input_Layer_Count);
            Hidden_Layer_Vector = new Vector(Hidden_Layer_Count);
            Output_Layer_Vector = new Vector(Output_Layer_Count);
            Template_Vector = new Vector(Output_Layer_Count);
            //Hidden_Offset_Vector = new Vector(Hidden_Layer_Count);
            //Hidden_Offset_Chang_Vector = new Vector(Hidden_Layer_Count);
            Output_Offset_Vector = new Vector(Output_Layer_Count);
            Output_Offset_Change_Vector = new Vector(Output_Layer_Count);

            Input_Hiddene_Coefficient_Matrix = new Matrix(Hidden_Layer_Count, Input_Layer_Count);
            Hidden_Output_Coefficient_Matrix = new Matrix(Output_Layer_Count, Hidden_Layer_Count);
            Input_Hidden_Coefficient_Change_Matrix = new Matrix(Hidden_Layer_Count, Input_Layer_Count);
            //Input_Hidden_Coefficient_Delta_Matrix = new Matrix(Hidden_Layer_Count, Input_Layer_Count);
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

            //for (int i = 0; i < Hidden_Layer_Count; i++)
            //    Hidden_Offset_Vector.item[i] = rnd.NextDouble() - 0.5;

            for (int k = 0; k < Output_Layer_Count; k++)
                Output_Offset_Vector.item[k] = rnd.NextDouble() - 0.5;

            for (int k = 0; k < Hidden_Layer_Count; k++)
            {
                Legendre legendre = new Legendre();
                legendre.K = k;
                FirstNeuronVector[k] = legendre;
            }
            for (int k = 0; k < Output_Layer_Count; k++)
            {
                Pureline pureline = new Pureline();
                SecondNeuronVector[k] = pureline;
            }

        }

        /// <summary>
        /// 根据输入向量，计算出输出向量。
        /// </summary>
        private void Calculate()
        {
            Hidden_Layer_Vector = Input_Hiddene_Coefficient_Matrix * Input_Layer_Vector;
            //Hidden_Layer_Vector.add(Hidden_Offset_Vector);
            for (int i = 0; i < Hidden_Layer_Count; i++)
            {
                Hidden_Layer_Vector.item[i] = FirstNeuronVector[i].NeuronFunction(Hidden_Layer_Vector.item[i]);
            }

            Output_Layer_Vector = Hidden_Output_Coefficient_Matrix * Hidden_Layer_Vector;
            Output_Layer_Vector.add(Output_Offset_Vector);
            for (int k = 0; k < Output_Layer_Count; k++)
            {
                Output_Layer_Vector.item[k] = SecondNeuronVector[k].NeuronFunction(Output_Layer_Vector.item[k]);
                Console.WriteLine("{0}={1}  ", k, Output_Layer_Vector.item[k].ToString());
            }
        }


        #region INeuralMatrix 成员


        public override void Load(string Filename)
        {
            FileStream fs = new FileStream(Filename, FileMode.Open);
            BinaryReader r = new BinaryReader(fs);
            this.coefficient = r.ReadDouble();
            this.offset = r.ReadDouble();
            this.Input_Layer_Count = r.ReadInt32();
            this.Hidden_Layer_Count = r.ReadInt32();
            this.Output_Layer_Count = r.ReadInt32();

            this.Input_Layer_Vector = new Vector(this.Input_Layer_Count);
            this.Hidden_Layer_Vector = new Vector(this.Hidden_Layer_Count);
            this.Output_Layer_Vector = new Vector(this.Output_Layer_Count);
            this.Template_Vector = new Vector(this.Output_Layer_Count);
            //this.Hidden_Offset_Vector = new Vector(this.Hidden_Layer_Count);
            //this.Hidden_Offset_Chang_Vector = new Vector(this.Hidden_Layer_Count);
            this.Output_Offset_Vector = new Vector(this.Output_Layer_Count);
            this.Output_Offset_Change_Vector = new Vector(this.Output_Layer_Count);

            this.Input_Hiddene_Coefficient_Matrix = new Matrix(this.Hidden_Layer_Count, this.Input_Layer_Count);
            this.Hidden_Output_Coefficient_Matrix = new Matrix(this.Output_Layer_Count, this.Hidden_Layer_Count);
            this.Input_Hidden_Coefficient_Change_Matrix = new Matrix(this.Hidden_Layer_Count, this.Input_Layer_Count);
            //this.Input_Hidden_Coefficient_Delta_Matrix = new Matrix(this.Hidden_Layer_Count, this.Input_Layer_Count);
            this.Hidden_Output_Coefficient_Change_Matrix = new Matrix(this.Output_Layer_Count, this.Hidden_Layer_Count);
            this.Hidden_Output_Coefficient_Delta_Matrix = new Matrix(this.Output_Layer_Count, this.Hidden_Layer_Count);

            for (int i = 0; i < this.Hidden_Layer_Count; i++)
                for (int j = 0; j < this.Input_Layer_Count; j++)
                    this.Input_Hiddene_Coefficient_Matrix.item[i, j] = r.ReadDouble();

            //for (int i = 0; i < this.Hidden_Layer_Count; i++)
            //    this.Hidden_Offset_Vector.item[i] = r.ReadDouble();

            for (int k = 0; k < this.Output_Layer_Count; k++)
                for (int i = 0; i < this.Hidden_Layer_Count; i++)
                    this.Hidden_Output_Coefficient_Matrix.item[k, i] = r.ReadDouble();

            for (int k = 0; k < this.Output_Layer_Count; k++)
                this.Output_Offset_Vector.item[k] = r.ReadDouble();
            r.Close();
            fs.Close();
        }

        public override void Save(string Filename)
        {
            FileStream fs = new FileStream(Filename, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(coefficient);
            w.Write(offset);
            w.Write((int)Input_Layer_Count);
            w.Write((int)Hidden_Layer_Count);
            w.Write((int)Output_Layer_Count);

            for (int i = 0; i < Hidden_Layer_Count; i++)
                for (int j = 0; j < Input_Layer_Count; j++)
                {
                    w.Write(Input_Hiddene_Coefficient_Matrix.item[i, j]);
                }


            //for (int i = 0; i < Hidden_Layer_Count; i++)
            //{
            //    w.Write(Hidden_Offset_Vector.item[i]);
            //}

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

        public override Vector Calculate(Vector vectorInput)
        {
            Input_Layer_Vector = vectorInput;
            Calculate();
            return Output_Layer_Vector;
        }

        public override void Training(Vector Input_Vector, Vector Template_Vector, int Loops)
        {
            this.Input_Layer_Vector = Input_Vector;
            this.Template_Vector = Template_Vector;
            for (int a = 0; a < Loops; a++)
            {
                Calculate();
                Console.WriteLine("Calculate finish: " + DateTime.Now.ToString());
                CalculateError();
                if (Delta > 1000000.0)
                    throw new Exception("So much error, stop training.");
                CalculateDelta_Hidden_Output_Coefficient();
                CalculateDelta_Input_Hidden_Coefficient();
                Console.WriteLine("CalculateDelta_Input_Hidden_Coefficient finish: " + DateTime.Now.ToString());
                Hidden_Output_Coefficient_Matrix.add(Hidden_Output_Coefficient_Change_Matrix);
                Input_Hiddene_Coefficient_Matrix.add(Input_Hidden_Coefficient_Change_Matrix);
                Console.WriteLine("Matrix adjust finish: " + DateTime.Now.ToString());
                //Hidden_Offset_Vector.add(Hidden_Offset_Chang_Vector);
                Output_Offset_Vector.add(Output_Offset_Change_Vector);
                Console.WriteLine("Vector adjust finish: " + DateTime.Now.ToString());
                if (Delta < 0.00000001)
                    break;
            }
        }

        #endregion

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
            for (int k = 0; k < Output_Layer_Count; k++)
            {
                double delta = SecondNeuronVector[k].NeuronFunctionDerivative(Output_Layer_Vector.item[k]) * (Template_Vector.item[k] - Output_Layer_Vector.item[k]);
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
            for (int i = 0; i < Hidden_Layer_Count; i++)
            {
                double delta = 0;
                for (int k = 0; k < Output_Layer_Count; k++)
                {
                    delta += Hidden_Output_Coefficient_Delta_Matrix.item[k, i] * Hidden_Output_Coefficient_Matrix.item[k, i];
                }
                delta *= FirstNeuronVector[i].NeuronFunctionDerivative(Hidden_Layer_Vector.item[i]);
                for (int j = 0; j < Input_Layer_Count; j++)
                {
                    //Input_Hidden_Coefficient_Delta_Matrix.item[i, j] = delta;
                    Input_Hidden_Coefficient_Change_Matrix.item[i, j] = Miu * delta * Input_Layer_Vector.item[j];
                }
                //Hidden_Offset_Chang_Vector.item[i] = delta * Miu;
            }
        }
    }
}
