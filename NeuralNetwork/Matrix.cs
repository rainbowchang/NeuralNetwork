using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    class Matrix
    {
        private double[,] Elements;
        public double[,] item
        {
            get
            {
                return Elements;
            }
        }
        public Matrix(int row, int col)
        {
            _RowNumber = row;
            _ColNumber = col;
            Elements = new double[row, col];
        }

        private int _RowNumber;
        /// <summary>
        /// 矩阵的行数
        /// </summary>
        public int RowNumber
        {
            get
            {
                return _RowNumber;
            }
        }

        private int _ColNumber;
        /// <summary>
        /// 矩阵的列数
        /// </summary>
        public int ColNumber
        {
            get
            {
                return _ColNumber;
            }
        }

        //public static Matrix operator +(Matrix m1, Matrix m2)
        //{
        //    Matrix result = new Matrix(m1.RowNumber, m1.ColNumber);
        //    for (int i = 0; i < m1.RowNumber; i++)
        //        for (int j = 0; j < m1.ColNumber; j++)
        //            result.item[i, j] = m1.item[i, j] + m2.item[i, j];
        //    return result;
        //}
        public void add(Matrix m2)
        {
            DateTime dt = DateTime.Now;
            for (int i = 0; i < this.RowNumber; i++)
                for (int j = 0; j < this.ColNumber; j++)
                    this.item[i, j] = this.item[i, j] + m2.item[i, j];
            TimeSpan dt2 = DateTime.Now - dt;
            Console.WriteLine("Matrix.add() spend " + dt2.ToString());
        }

    }

    class Vector
    {
        private int _Number ;
        public int UpperBound
        {
            get
            {
                return _Number;
            }
        }
        private double[] Elements;
        public double[] item
        {
            get { return Elements; }
        }
        public Vector(int n)
        {
            _Number = n;
            Elements = new double[_Number];
        }

        /// <summary>
        /// 两个向量相加
        /// </summary>
        /// <param name="Vector1"></param>
        /// <param name="Vector2"></param>
        /// <returns></returns>
        //public static Vector operator + (Vector Vector1, Vector Vector2)
        //{
        //    if (Vector1.UpperBound != Vector2.UpperBound)
        //        throw new Exception("Two Vectors are not same upper bound");

        //    int length = Vector1.UpperBound;
        //    Vector result = new Vector(length);

        //    for (int x = 0; x < length; x++)
        //        result.Elements[x] = Vector1.Elements[x] + Vector2.Elements[x];

        //    return result;
        //}

        public void add(Vector Vector2)
        {
            int length = this.UpperBound;
            for (int x = 0; x < length; x++)
                this.Elements[x] = this.Elements[x] + Vector2.Elements[x];
        }


        /// <summary>
        /// 计算矩阵和向量的乘积, 矩阵与一个纵向量的积，结果是一个纵向量。
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector operator *(Matrix matrix, Vector vector)
        {
            DateTime dt = DateTime.Now;
            if (matrix.ColNumber != vector.UpperBound)
                throw new Exception("Matrix and Vector are not same upperbound.");

            int height = matrix.RowNumber;
            int width = matrix.ColNumber;

            Vector result = new Vector(height);

            for (int x = 0; x < height; x++)
            {
                double k = 0;
                for (int y = 0; y < width; y++)
                {
                    k += matrix.item[x,y] * vector.item[y];
                }
                result.item[x] = k;
            }
            TimeSpan dt2 = DateTime.Now - dt;
            Console.WriteLine("Vector.multi() spend " + dt2.ToString());
            return result;
        }
    }
}
