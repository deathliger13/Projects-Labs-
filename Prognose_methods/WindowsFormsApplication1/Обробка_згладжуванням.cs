using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Обробка_згладжуванням
    {
        private double[] y;
        private int M, L;

        public double[] Y
        {
            get
            {
                return Y;
            }
            set
            {
                y = value;
            }
        }

        public Обробка_згладжуванням(double[] x, double[] y, int M, int L)
        {
            Y = y;
            this.M = M;
            this.L = L;
        }

        public double[] Функція_згладжування()
        {
            double[] y_згладжуване = new double[M - L + 1];
            double s;
            for (int i = 0; i < M-L+1; i++)
            {
                s = 0.0;
                for (int j = i; j < i + L; j++)
                    s += y[j];
                y_згладжуване[i] = s/L;
            }
            return y_згладжуване;
        }
    }
}
