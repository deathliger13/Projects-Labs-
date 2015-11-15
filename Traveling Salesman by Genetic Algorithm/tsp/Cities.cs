// КПИ Дорошенко
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
    /// Этот класс содержит список городов для этого теста.
    /// Каждый город имеет место и информацию о расстоянии до любой другой город.
    /// </summary>
    public class Cities : List<City>
    {
        /// <summary>
        /// Determine the distances between each city.
        /// </summary>
        /// <param name="numberOfCloseCities">При создании начальной популяции туров, это больше шансов
        /// что неподалеку город будет выбран для связи. Это число близлежащих городов, которые будут рассмотрены близко.</param>
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
        /// Открыть файл ХМЛ где находиться список городов.
        /// </summary>
        /// <param name="fileName">Название файла.</param>
        /// <returns>Список городов.</returns>
        /// <exception cref="FileNotFoundException">Неправильное имя.</exception>
        /// <exception cref="InvalidCastException">неправильный формат.</exception>
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
