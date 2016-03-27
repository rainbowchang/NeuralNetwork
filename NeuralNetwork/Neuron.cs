using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    interface INeuron
    {
        /// <summary>
        /// 激活函数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double NeuronFunction(double x);

        /// <summary>
        /// 激活函数的导数
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double NeuronFunctionDerivative(double x);
    }

    /// <summary>
    /// 对数-S型函数
    /// </summary>
    class Logsig : INeuron
    {
        public double NeuronFunction(double x)
        {
            double y = 1.0 / (1 + Math.Exp(x * (-1)));
            return y;
        }

        public double NeuronFunctionDerivative(double x)
        {
            double a = Math.Exp(x);
            double y = a / ((1 + a) * (1 + a));
            return y;
        }
    }

    /// <summary>
    /// 双曲正切S型函数
    /// </summary>
    class Tansig : INeuron
    {
        public double NeuronFunction(double x)
        {
            return Math.Tanh(x);
        }

        public double NeuronFunctionDerivative(double x)
        {
            double y;
            y = NeuronFunction(x);
            y = 1 - y * y;
            return y;
        }
    }

    /// <summary>
    /// 线形函数
    /// </summary>
    class Pureline : INeuron
    {
        public double NeuronFunction(double x)
        {
            return x;
        }

        public double NeuronFunctionDerivative(double x)
        {
            return 1.0;
        }
    }

    class Legendre : INeuron
    {
        private int k;
        public int K
        {
            set
            {
                k = value;
            }
        }
        public double NeuronFunction(double x)
        {
            return NeuronFunction(x, k);
        }
        private double NeuronFunction(double x, int k)
        {
            if (k < 0)
                throw new Exception("Function k must >= 0 .");
            else if (k == 0)
                return 1.0;
            else if (k == 1)
                return x;
            else
                return ((NeuronFunction(x, (k - 1))) * (2.0 * k - 1) * x / (k) - NeuronFunction(x, (k - 2)) * (k - 1) / k);
        }

        public double NeuronFunctionDerivative(double x)
        {
            return NeuronFunctionDerivative(x, k);
        }
        private double NeuronFunctionDerivative(double x, int k)
        {
            if (k < 0)
                throw new Exception("Function k must >= 0 .");
            else if (k == 0)
                return 0.0;
            else if (k == 1)
                return 1.0;
            else
                return ((NeuronFunction(x, (k - 1))) * (2.0 * k - 1) / (k) + (NeuronFunctionDerivative(x, (k - 1))) * (2.0 * k - 1) * x / (k) - NeuronFunctionDerivative(x, (k - 2)) * (k - 1) / k);
        }
    }
}
