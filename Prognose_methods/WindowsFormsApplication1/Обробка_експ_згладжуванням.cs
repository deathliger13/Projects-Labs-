using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Обробка_експ_згладжуванням
    {
        private double[] y;
        private int M, L;
        private double alpha;

        public double [] Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public Обробка_експ_згладжуванням(double[] y, int M, double alpha, int L)
        {
            Y = y;
            this.M = M;
            this.alpha = alpha;
            this.L = L;
        }

        public double[] Функція_згладжування()
        {
            double[] y_згладжуване = new double[M - L + 1];
            double s;
            for (int i = 0; i < M - L + 1; i++)
            {
                s = 0.0;
                for (int j = i+1; j < i + L; j++)
                    s = alpha * y[j] + (1-alpha)*y[j-1];
                y_згладжуване[i] = s;
            }
            return y_згладжуване;
        }
    }
}
