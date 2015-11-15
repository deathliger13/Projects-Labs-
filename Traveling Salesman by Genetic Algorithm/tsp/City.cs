//ЧНУ Головин
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tsp
{
    /// <summary>
    /// Индивидуальные города.
    /// </summary>
    public class City
    {
        /// <summary>
        /// Конструктор маршрута.
        /// </summary>
        /// <param name="x">X позиция города.</param>
        /// <param name="y">Y позиция города.</param>
        public City(int x, int y)
        {
            Location = new Point(x, y);
        }

        /// <summary>
        /// ЗАКРЫТАЯ КОПИЯ МЕСТОПОЛОЖЕНИЯ ГОРОДА.
        /// </summary>
        private Point location;
        /// <summary>
        /// Положение города.
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
        /// Личная копия расстояния от этого города и любой другой город.
        /// Индекс в массиве это количество города, связанного с...
        /// </summary>
        private List<double> distances = new List<double>();
        /// <summary>
        /// Расстояние от 1 города к любому другому.
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
        /// Личная копия списка городов, которые ближе к этому.
        /// </summary>
        private List<int> closeCities = new List<int>();
        /// <summary>
        /// Список городов, которые ближе к этому.
        /// </summary>
        public List<int> CloseCities
        {
            get
            {
                return closeCities;
            }
        }

        /// <summary>
        /// Поиск городов которые ближе к этому городу.
        /// </summary>
        /// <param name="numberOfCloseCities">При создании начальной популяции туров, это больше шансов
        /// что неподалеку город будет выбран для связи. Это число близлежащих городов, которые будут считать близким.</param>
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
