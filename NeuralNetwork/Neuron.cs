﻿using System;
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
        public double NeuronFunction(double x){
            double y = 1.0 / (1 + Math.Exp(x * (-1)));
            return y;
        }

        public double NeuronFunctionDerivative(double x)
        {
            double y = Math.Exp(x) / ((1 + Math.Exp(x)) * (1 + Math.Exp(x)));
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
}