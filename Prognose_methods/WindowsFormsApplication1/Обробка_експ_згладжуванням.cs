using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class �������_����_�������������
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

        public �������_����_�������������(double[] y, int M, double alpha, int L)
        {
            Y = y;
            this.M = M;
            this.alpha = alpha;
            this.L = L;
        }

        public double[] �������_������������()
        {
            double[] y_����������� = new double[M - L + 1];
            double s;
            for (int i = 0; i < M - L + 1; i++)
            {
                s = 0.0;
                for (int j = i+1; j < i + L; j++)
                    s = alpha * y[j] + (1-alpha)*y[j-1];
                y_�����������[i] = s;
            }
            return y_�����������;
        }
    }
}
