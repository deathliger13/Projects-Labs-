//КПИ Дорошенко, ЧНУ Головин
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsp
{
    /// <summary>
    /// Этот класс представляет один экземпляр тура по всем городам.
    /// </summary>
    public class Tour : List<Link>
    {
        /// <summary>
        ///Конструктор, который принимает мощность по умолчанию.
        /// </summary>
        /// <param name="capacity">Начальный размер тура. Должен быть ряд городов в турне.</param>
        public Tour(int capacity)
            : base(capacity)
        {
            resetTour(capacity);
        }

        /// <summary>
        /// Личная копия пригодности этого тура.Фитнес функция.
        /// </summary>
        private double fitness;
        /// <summary>
        /// Фитнес фннкция (общая длина тура) этого тура
        /// </summary>
        public double Fitness
        {
            set
            {
                fitness = value;
            }
            get
            {
                return fitness;
            }
        }

        /// <summary>
        /// Создает тур с правильным количеством городов и создает начальные подключения всех -1.
        /// </summary>
        /// <param name="numberOfCities"></param>
        private void resetTour(int numberOfCities)
        {
            this.Clear();

            Link link;
            for (int i = 0; i < numberOfCities; i++)
            {
                link = new Link();
                link.Connection1 = -1;
                link.Connection2 = -1;
                this.Add(link);
            }
        }

        /// <summary>
        /// Определяет пригодность (общая длина) отдельного тура.
        /// </summary>
        /// <param name="cities">Города в этом туре. Используется для получения расстояния между каждом городе.</param>
        public void DetermineFitness(Cities cities)
        {
            Fitness = 0;

            int lastCity = 0;
            int nextCity = this[0].Connection1;

            foreach (Link link in this)
            {
                Fitness += cities[lastCity].Distances[nextCity];

                // выясняет, если следующий  город в списке [0] или [1]
                if (lastCity != this[nextCity].Connection1)
                {
                    lastCity = nextCity;
                    nextCity = this[nextCity].Connection1;
                }
                else
                {
                    lastCity = nextCity;
                    nextCity = this[nextCity].Connection2;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="tour">Незаконченый "Детский" тур.</param>
        /// <param name="cityUsage"</param>
        /// <param name="city1">1 город "Вконтакте".</param>
        /// <param name="city2">2 город "Вконтакте".</param>
        private static void joinCities(Tour tour, int[] cityUsage, int city1, int city2)
        {
            if (tour[city1].Connection1 == -1)
            {
                tour[city1].Connection1 = city2;
            }
            else
            {
                tour[city1].Connection2 = city2;
            }

            if (tour[city2].Connection1 == -1)
            {
                tour[city2].Connection1 = city1;
            }
            else
            {
                tour[city2].Connection2 = city1;
            }

            cityUsage[city1]++;
            cityUsage[city2]++;
        }


        /// <summary>
        /// Найти ссылку из данного города в родительском туре, которые могут быть размещены в туре ребенка.
        /// Если обе ссылки родителя не являются допустимыми ссылками на тур с детьми, вернуть -1.
        /// </summary>
        /// <param name="parent">Родительский тур</param>
        /// <param name="child">Замещёный детский тур.</param>
        /// <param name="cityList">Список городов в туре.</param>
        /// <param name="cityUsage">Количество"Детских" городов.</param>
        /// <param name="city">Город с которым хотим сконтачить.</param>
        /// <returns>Детский тур.</returns>
        private static int findNextCity(Tour parent, Tour child, Cities cityList, int[] cityUsage, int city)
        {
            if (testConnectionValid(child, cityList, cityUsage, city, parent[city].Connection1))
            {
                return parent[city].Connection1;
            }
            else if (testConnectionValid(child, cityList, cityUsage, city, parent[city].Connection2))
            {
                return parent[city].Connection2;
            }

            return -1;
        }

        /// <summary>
        /// Если два города могут быть подключены уже (witout делать полный тур), то это недопустимая ссылка.
        /// </summary>
        /// <param name="tour">Незаконченый "Детский" тур.</param>
        /// <param name="cityList">Список городов.</param>
        /// <param name="cityUsage">Массив, который содержит количество раз когда каждый город был связан.</param>
        /// <param name="city1">1 город "Вконтакте)))".</param>
        /// <param name="city2">2 город "Вконтакте)))"..</param>
        /// <returns>True Если соединение возможно.</returns>
        private static bool testConnectionValid(Tour tour, Cities cityList, int[] cityUsage, int city1, int city2)
        {
            if ((city1 == city2) || (cityUsage[city1] == 2) || (cityUsage[city2] == 2))
            {
                return false;
            }

            if ((cityUsage[city1] == 0) || (cityUsage[city2] == 0))
            {
                return true;
            }

            //Надо увидеть если города связаны и собираны в каждом направлении.
            for (int direction = 0; direction < 2; direction++)
            {
                int lastCity = city1;
                int currentCity;
                if (direction == 0)
                {
                    currentCity = tour[city1].Connection1;  //1 "Контакт"
                }
                else
                {
                    currentCity = tour[city1].Connection2;  // 2 "Контакт"
                }
                int tourLength = 0;
                while ((currentCity != -1) && (currentCity != city2) && (tourLength < cityList.Count - 2))
                {
                    tourLength++;
                    
                    if (lastCity != tour[currentCity].Connection1)
                    {
                        lastCity = currentCity;
                        currentCity = tour[currentCity].Connection1;
                    }
                    else
                    {
                        lastCity = currentCity;
                        currentCity = tour[currentCity].Connection2;
                    }
                }

                // если города связаны, но это проходит через каждый город в списке, затем ОК, чтобы присоединиться.
                if (tourLength >= cityList.Count - 2)
                {
                    return true;
                }

                // Если города связаны, минуя все города, это не ОК, чтобы присоединиться.
                if (currentCity == city2)
                {
                    return false;
                }
            }

            // если города не были связаны собираемся в любом направлении, ОК, чтобы присоединиться к ним
            return true;
        }

        /// <summary>
        /// Выполните операцию кроссовера на 2 родительских туров, чтобы создать новый тур ребенка.
        /// Эта функция должна быть вызвана два раза, чтобы сделать 2 детей.
        /// По второму призыву зщначение родителей должно быть изменено
        /// </summary>
        /// <param name="parent1">1 путишествие</param>
        /// <param name="parent2">" путишествие.</param>
        /// <param name="cityList">Список городов в туре.</param>
        /// <param name="rand">.</param>
        /// <returns>The child tour.</returns>
        public static Tour Crossover(Tour parent1, Tour parent2, Cities cityList, Random rand)
        {
            Tour child = new Tour(cityList.Count);      // Новый тур
            int[] cityUsage = new int[cityList.Count];  // Количество связей с "Городом"
            int city;                                   
            int nextCity;                               // Слудеющий город присоединён

            for (city = 0; city < cityList.Count; city++)
            {
                cityUsage[city] = 0;
            }

            // Возьмём все ссылки, на которые оба родителя согласны, и положим их в ребенка
            for (city = 0; city < cityList.Count; city++)
            {
                if (cityUsage[city] < 2)
                {
                    if (parent1[city].Connection1 == parent2[city].Connection1)
                    {
                        nextCity = parent1[city].Connection1;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection2 == parent2[city].Connection2)
                    {
                        nextCity = parent1[city].Connection2;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);

                        }
                    }
                    if (parent1[city].Connection1 == parent2[city].Connection2)
                    {
                        nextCity = parent1[city].Connection1;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);
                        }
                    }
                    if (parent1[city].Connection2 == parent2[city].Connection1)
                    {
                        nextCity = parent1[city].Connection2;
                        if (testConnectionValid(child, cityList, cityUsage, city, nextCity))
                        {
                            joinCities(child, cityUsage, city, nextCity);
                        }
                    }
                }
            }

            // Родители не согласны на что осталось, так что мы будем чередовать с помощью
            // Ссылок из родителей 1, а затем родителей 2.

            for (city = 0; city < cityList.Count; city++)
            {
                if (cityUsage[city] < 2)
                {
                    if (city % 2 == 1)  // мы предпочитаем родителя 1
                    {
                        nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                        if (nextCity == -1) // но если это не возможно, мы по-прежнему идём с родителем 2
                        {
                            nextCity = findNextCity(parent2, child, cityList, cityUsage, city); ;
                        }
                    }
                    else // Родитель 2
                    {
                        nextCity = findNextCity(parent2, child, cityList, cityUsage, city);
                        if (nextCity == -1)
                        {
                            nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                        }
                    }

                    if (nextCity != -1)
                    {
                        joinCities(child, cityUsage, city, nextCity);

                        // еще этого не сделали. должен был быть 0 в предыдущем случае.
                        if (cityUsage[city] == 1)
                        {
                            if (city % 2 != 1)  // Родитель 1
                            {
                                nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                                if (nextCity == -1) // Родитель 2 
                                {
                                    nextCity = findNextCity(parent2, child, cityList, cityUsage, city);
                                }
                            }
                            else // Родитель 2
                            {
                                nextCity = findNextCity(parent2, child, cityList, cityUsage, city);
                                if (nextCity == -1)
                                {
                                    nextCity = findNextCity(parent1, child, cityList, cityUsage, city);
                                }
                            }

                            if (nextCity != -1)
                            {
                                joinCities(child, cityUsage, city, nextCity);
                            }
                        }
                    }
                }
            }

            // Оставушиеся связи должны быть рандомными.
            // Ссылки на Родителя вызовут многочисленные разрозненные петли.
            for (city = 0; city < cityList.Count; city++)
            {
                while (cityUsage[city] < 2)
                {
                    do
                    {
                        nextCity = rand.Next(cityList.Count);  // выбраем случайный город, пока мы не найдем тот, с которым мы можем связатся 
                    } while (!testConnectionValid(child, cityList, cityUsage, city, nextCity));

                    joinCities(child, cityUsage, city, nextCity);
                }
            }

            return child;
        }

        /// <summary>
        /// Случайно изменяет одну из ссылок в этом туре.
        /// </summary>
        /// <param name="rand">Генератор случайных чисел. Мы проходим примернно, так что результаты между трасс соответствуют.</param>
        public void Mutate(Random rand)
        {
            int cityNumber = rand.Next(this.Count);
            Link link = this[cityNumber];
            int tmpCityNumber;

            // Находит 2 города подключеные к cityNumber, а затем подключает их непосредственно
            if (this[link.Connection1].Connection1 == cityNumber)   // Conn 1 on Conn 1 link points back to us.
            {
                if (this[link.Connection2].Connection1 == cityNumber)// Conn 1 on Conn 2 link points back to us.
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection1 =link.Connection1;
                    this[link.Connection1].Connection1 = tmpCityNumber;
                }
                else                                                // Conn 2 on Conn 2 link points back to us.
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection2 = link.Connection1;
                    this[link.Connection1].Connection1 = tmpCityNumber;
                }
            }
            else                                                    // Conn 2 on Conn 1 link points back to us.
            {
                if (this[link.Connection2].Connection1 == cityNumber)// Conn 1 on Conn 2 link points back to us.
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection1 = link.Connection1;
                    this[link.Connection1].Connection2 = tmpCityNumber;
                }
                else                                                // Conn 2 on Conn 2 link points back to us.
                {
                    tmpCityNumber = link.Connection2;
                    this[link.Connection2].Connection2 = link.Connection1;
                    this[link.Connection1].Connection2 = tmpCityNumber;
                }

            }

            int replaceCityNumber = -1;
            do
            {
                replaceCityNumber = rand.Next(this.Count);
            }
            while (replaceCityNumber == cityNumber);
            Link replaceLink = this[replaceCityNumber];

            // Теперь мы должны вставить этот город обратно в турне в случайном месте
            tmpCityNumber = replaceLink.Connection2;
            link.Connection2 = replaceLink.Connection2;
            link.Connection1 = replaceCityNumber;
            replaceLink.Connection2 = cityNumber;

            if (this[tmpCityNumber].Connection1 == replaceCityNumber)
            {
                this[tmpCityNumber].Connection1 = cityNumber;
            }
            else
            {
                this[tmpCityNumber].Connection2 = cityNumber;
            }
        }
    }
}
