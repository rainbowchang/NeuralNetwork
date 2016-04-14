using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NeuralNetwork
{
    public interface INeuralMatrix
    {
        void Load(String Filename);
        void Save(String Filename);
        Vector Calculate(Vector vectorInput);
        void Training(Vector Input_Vector, Vector Template_Vector, int Loops);
    }

    public abstract class NeuralMatrix : INeuralMatrix
    {

        public abstract void Load(String Filename);
        public abstract void Save(String Filename);
        public abstract Vector Calculate(Vector vectorInput);
        public abstract void Training(Vector Input_Vector, Vector Template_Vector, int Loops);
        /// <summary>
        /// 输入向量归一化时候的系数
        /// </summary>
        public double coefficient = 0.0;
        /// <summary>
        /// 输入向量归一化时候的偏移量
        /// </summary>
        public double offset = 0.0;

        /// <summary>
        /// 输入向量
        /// </summary>
        protected Vector Input_Layer_Vector;
        /// <summary>
        /// 隐含层输出向量
        /// </summary>
        protected Vector Hidden_Layer_Vector;
        /// <summary>
        /// 输出向量
        /// </summary>
        protected Vector Output_Layer_Vector;
        /// <summary>
        /// 预期结果向量
        /// </summary>
        protected Vector Template_Vector;
        /// <summary>
        /// 隐含层偏移量
        /// </summary>
        protected Vector Hidden_Offset_Vector;
        /// <summary>
        /// 隐含层偏移量调整量
        /// </summary>
        protected Vector Hidden_Offset_Chang_Vector;
        /// <summary>
        /// 输出层偏移量
        /// </summary>            
        protected Vector Output_Offset_Vector;
        /// <summary>
        /// 输出层偏移量调整量
        /// </summary>
        protected Vector Output_Offset_Change_Vector;

        /// <summary>
        /// 输入层单元数量
        /// </summary>
        protected int Input_Layer_Count;
        /// <summary>
        /// 隐含层单元数量
        /// </summary>
        protected int Hidden_Layer_Count;
        /// <summary>
        /// 输出层单元数量
        /// </summary>
        protected int Output_Layer_Count;

        /// <summary>
        /// 输入层到隐含层的系数矩阵
        /// </summary>
        protected Matrix Input_Hiddene_Coefficient_Matrix;
        /// <summary>
        /// 隐含层到输出层的系数矩阵
        /// </summary>
        protected Matrix Hidden_Output_Coefficient_Matrix;
        /// <summary>
        /// 输入层到隐含层的系数偏移矩阵
        /// </summary>
        protected Matrix Input_Hidden_Coefficient_Change_Matrix;
        /// <summary>
        /// 输入层到隐含层的Delta系数矩阵
        /// </summary>
        //protected Matrix Input_Hidden_Coefficient_Delta_Matrix;
        /// <summary>
        /// 隐含层到输出层的系数偏移矩阵
        /// </summary>
        protected Matrix Hidden_Output_Coefficient_Change_Matrix;
        /// <summary>
        /// 输入层到隐含层的Delta系数矩阵
        /// </summary>
        protected Matrix Hidden_Output_Coefficient_Delta_Matrix;
        /// <summary>
        /// 第一层神经元向量
        /// </summary>
        protected INeuron[] FirstNeuronVector;
        /// <summary>
        /// 第二层神经元向量
        /// </summary>
        protected INeuron[] SecondNeuronVector;
        /// <summary>
        /// 学习步长
        /// </summary>
        protected double Miu;
        /// <summary>
        /// 误差
        /// </summary>
        protected double Delta;
    }

}
