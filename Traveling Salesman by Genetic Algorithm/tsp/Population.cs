// ЧНУ Головин
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tsp
{
    class Population : List<Tour>
    {
        /// <summary>
        /// Личная копия лучшего тура который нашёл до сих пор генетический алгоритм.
        /// </summary>
        private Tour bestTour = null;
        /// <summary>
        /// Лучший тур который нашёл до сих пор генетический алгоритм.
        /// </summary>
        public Tour BestTour
        {
            set
            {
                bestTour = value;
            }
            get
            {
                return bestTour;
            }
        }

        /// <summary>
        /// Создать начальный набор случайных туров.
        /// </summary>
        /// <param name="populationSize">Число туров для создания.</param>
        /// <param name="cityList">Список городов в туре.</param>
        /// <param name="rand">Генератор случайных чисел. Мы проходим примерно, так что результаты между трасс соответствуют.</param>
        /// <param name="chanceToUseCloseCity">Шансы (из 100), что город, который, как известно, чтобы быть близко будет использоваться в любой данной линии связи.</param>
        public void CreateRandomPopulation(int populationSize, Cities cityList, Random rand, int chanceToUseCloseCity)
        {
            int firstCity, lastCity, nextCity;

            for (int tourCount = 0; tourCount < populationSize; tourCount++)
            {
                Tour tour = new Tour(cityList.Count);

                // Начинаем создавать начальную точку для этого тура
                firstCity = rand.Next(cityList.Count);
                lastCity = firstCity;

                for (int city = 0; city < cityList.Count - 1; city++)
                {
                    do
                    {
                        // Держит выбор случайных городов для следующего города, пока мы не найдем тот,в котором мы не были.
                        if ((rand.Next(100) < chanceToUseCloseCity) && ( cityList[city].CloseCities.Count > 0 ))
                        {
                            // 75% шанс будет подбран город, который близок к этому
                            nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
                        }
                        else
                        {
                            // В противном случае, выбираем город полностью случайный.
                            nextCity = rand.Next(cityList.Count);
                        }
                        //Убедились, что нас здесь не было, и убедились, что это не там, где мы теперь.
                    } while ((tour[nextCity].Connection2 != -1) || (nextCity == lastCity));

                    // Когда идём с города A в B, [1] на A = B и [1] на город B = A
                    tour[lastCity].Connection2 = nextCity;
                    tour[nextCity].Connection1 = lastCity;
                    lastCity = nextCity;
                }

                // Соединяет два последних города
                tour[lastCity].Connection2 = firstCity;
                tour[firstCity].Connection1 = lastCity;

                tour.DetermineFitness(cityList);

                Add(tour);

                if ((bestTour == null) || (tour.Fitness < bestTour.Fitness))
                {
                    BestTour = tour;
                }
            }
        }
    }
}
