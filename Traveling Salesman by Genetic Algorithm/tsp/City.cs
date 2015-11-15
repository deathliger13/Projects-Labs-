//��� �������
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tsp
{
    /// <summary>
    /// �������������� ������.
    /// </summary>
    public class City
    {
        /// <summary>
        /// ����������� ��������.
        /// </summary>
        /// <param name="x">X ������� ������.</param>
        /// <param name="y">Y ������� ������.</param>
        public City(int x, int y)
        {
            Location = new Point(x, y);
        }

        /// <summary>
        /// �������� ����� �������������� ������.
        /// </summary>
        private Point location;
        /// <summary>
        /// ��������� ������.
        /// </summary>
        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// ������ ����� ���������� �� ����� ������ � ����� ������ �����.
        /// ������ � ������� ��� ���������� ������, ���������� �...
        /// </summary>
        private List<double> distances = new List<double>();
        /// <summary>
        /// ���������� �� 1 ������ � ������ �������.
        /// </summary>
        public List<double> Distances
        {
            get
            {
                return distances;
            }
            set
            {
                distances = value;
            }
        }

        /// <summary>
        /// ������ ����� ������ �������, ������� ����� � �����.
        /// </summary>
        private List<int> closeCities = new List<int>();
        /// <summary>
        /// ������ �������, ������� ����� � �����.
        /// </summary>
        public List<int> CloseCities
        {
            get
            {
                return closeCities;
            }
        }

        /// <summary>
        /// ����� ������� ������� ����� � ����� ������.
        /// </summary>
        /// <param name="numberOfCloseCities">��� �������� ��������� ��������� �����, ��� ������ ������
        /// ��� ���������� ����� ����� ������ ��� �����. ��� ����� ����������� �������, ������� ����� ������� �������.</param>
        public void FindClosestCities( int numberOfCloseCities )
        {
            double shortestDistance;
            int shortestCity = 0;
            double[] dist = new double[Distances.Count];
            Distances.CopyTo(dist);

            if (numberOfCloseCities > Distances.Count - 1)
            {
                numberOfCloseCities = Distances.Count - 1;
            }

            closeCities.Clear();

            for (int i = 0; i < numberOfCloseCities; i++)
            {
                shortestDistance = Double.MaxValue;
                for (int cityNum = 0; cityNum < Distances.Count; cityNum++)
                {
                    if (dist[cityNum] < shortestDistance)
                    {
                        shortestDistance = dist[cityNum];
                        shortestCity = cityNum;
                    }
                }
                closeCities.Add(shortestCity);
                dist[shortestCity] = Double.MaxValue;
            }
        }
    }
}
