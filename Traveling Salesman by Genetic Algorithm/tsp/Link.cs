//���� �����������
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsp
{
    /// <summary>
    /// �������������� ����� ����� 2 �������� � ����.
    /// ���� ����� ��������� 2 ������ ������.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// ���������� � "������" �������.
        /// </summary>
        private int connection1;
        /// <summary>
        /// ���������� � "������" �������.
        /// </summary>
        public int Connection1
        {
            get
            {
                return connection1;
            }
            set
            {
                connection1 = value; ;
            }
        }

        /// <summary>
        /// ���������� � "������" �������.
        /// </summary>
        private int connection2;
        /// <summary>
        /// ���������� � "������" �������.
        /// </summary>
        public int Connection2
        {
            get
            {
                return connection2;
            }
            set
            {
                connection2 = value;
            }
        }
    }
}
