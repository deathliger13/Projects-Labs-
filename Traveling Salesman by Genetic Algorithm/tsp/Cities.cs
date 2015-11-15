// ��� ���������
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Globalization;

namespace Tsp
{
    /// <summary>
    /// ���� ����� �������� ������ ������� ��� ����� �����.
    /// ������ ����� ����� ����� � ���������� � ���������� �� ����� ������ �����.
    /// </summary>
    public class Cities : List<City>
    {
        /// <summary>
        /// Determine the distances between each city.
        /// </summary>
        /// <param name="numberOfCloseCities">��� �������� ��������� ��������� �����, ��� ������ ������
        /// ��� ���������� ����� ����� ������ ��� �����. ��� ����� ����������� �������, ������� ����� ����������� ������.</param>
        public void CalculateCityDistances( int numberOfCloseCities )
        {
            foreach (City city in this)
            {
                city.Distances.Clear();

                for (int i = 0; i < Count; i++)
                {
                    city.Distances.Add(Math.Sqrt(Math.Pow((double)(city.Location.X - this[i].Location.X), 2D) +
                                       Math.Pow((double)(city.Location.Y - this[i].Location.Y), 2D)));
                }
            }

            foreach (City city in this)
            {
                city.FindClosestCities(numberOfCloseCities);
            }
        }

        /// <summary>
        /// ������� ���� ��� ��� ���������� ������ �������.
        /// </summary>
        /// <param name="fileName">�������� �����.</param>
        /// <returns>������ �������.</returns>
        /// <exception cref="FileNotFoundException">������������ ���.</exception>
        /// <exception cref="InvalidCastException">������������ ������.</exception>
        public void OpenCityList(string fileName)
        {
            DataSet cityDS = new DataSet();

            try
            {
                this.Clear();

                cityDS.ReadXml(fileName);

                DataRowCollection cities = cityDS.Tables[0].Rows;

                foreach (DataRow city in cities)
                {
                    this.Add(new City(Convert.ToInt32(city["X"], CultureInfo.CurrentCulture), Convert.ToInt32(city["Y"], CultureInfo.CurrentCulture)));
                }
            }
            finally
            {
                cityDS.Dispose();
            }
        }
    }
}
