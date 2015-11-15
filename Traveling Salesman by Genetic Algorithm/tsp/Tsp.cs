//КПИ Середа, ЧДТУ Варварецкий
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Drawing;

namespace Tsp
{
    /// <summary>
    /// Этот класс выполняет алгоритм коммивояжера.
    /// </summary>
    class Tsp
    {
        /// <summary>
        /// Делегат используется для повышения событие, когда новый лучший тур найден.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NewBestTourEventHandler(Object sender, TspEventArgs e);

        /// <summary>
        /// </summary>
        public event NewBestTourEventHandler foundNewBestTour;

        /// <summary>
        /// </summary>
        Random rand;

        /// <summary>
        /// </summary>
        Cities cityList;

        /// <summary>
        /// </summary>
        Population population;

        /// <summary>
        /// </summary>
        private bool halt = false;
        /// <summary>
        /// </summary>
        public bool Halt
        {
            get
            {
                return halt;
            }
            set
            {
                halt = value;
            }
        }

        /// <summary>
        /// </summary>
        public Tsp()
        {
        }

        /// <summary>
        /// <see cref="Halt"/> 
        /// </summary>
        /// <param name="populationSize">рандом</param>
        /// <param name="maxGenerations">Кроссовер</param>
        /// <param name="groupSize">Выбор родителей и создание детей</param>
        /// <param name="mutation">Мутации</param>
        /// <param name="seed">Розсеивание</param>
        /// <param name="chanceToUseCloseCity">Ближайший город</param>
        /// <param name="cityList">Список городов.</param>
        public void Begin(int populationSize, int maxGenerations, int groupSize, int mutation, int seed, int chanceToUseCloseCity, Cities cityList)
        {
            rand = new Random(seed);

            this.cityList = cityList;

            population = new Population();
            population.CreateRandomPopulation(populationSize, cityList, rand, chanceToUseCloseCity);

            displayTour(population.BestTour, 0, false);

            bool foundNewBestTour = false;
            int generation;
            for (generation = 0; generation < maxGenerations; generation++)
            {
                if (Halt)
                {
                    break;  
                }
                foundNewBestTour = makeChildren(groupSize, mutation);

                if (foundNewBestTour)
                {
                    displayTour(population.BestTour, generation, false);
                }
            }

            displayTour(population.BestTour, generation, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="groupSize">Число туров</param>
        /// <param name="mutation">Мутирование наследника.</param>
        bool makeChildren(int groupSize, int mutation)
        {
            int[] tourGroup = new int[groupSize];
            int tourCount, i, topTour, childPosition, tempTour;
        	
            for (tourCount = 0; tourCount < groupSize; tourCount++)
            {
                tourGroup[tourCount] = rand.Next(population.Count);
            }

            // bubble sort 
            for (tourCount = 0; tourCount < groupSize - 1; tourCount++)
            {
                topTour = tourCount;
                for (i = topTour + 1; i < groupSize; i++)
                {
                    if (population[tourGroup[i]].Fitness < population[tourGroup[topTour]].Fitness)
                    {
                        topTour = i;
                    }
                }

                if (topTour != tourCount)
                {
                    tempTour = tourGroup[tourCount];
                    tourGroup[tourCount] = tourGroup[topTour];
                    tourGroup[topTour] = tempTour;
                }
            }

            bool foundNewBestTour = false;

            // " лутших пути для кросовера
            childPosition = tourGroup[groupSize - 1];
            population[childPosition] = Tour.Crossover(population[tourGroup[0]], population[tourGroup[1]], cityList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineFitness(cityList);

            if (population[childPosition].Fitness < population.BestTour.Fitness)
            {
                population.BestTour = population[childPosition];
                foundNewBestTour = true;
            }

            childPosition = tourGroup[groupSize - 2];
            population[childPosition] = Tour.Crossover(population[tourGroup[1]], population[tourGroup[0]], cityList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineFitness(cityList);

            if (population[childPosition].Fitness < population.BestTour.Fitness)
            {
                population.BestTour = population[childPosition];
                foundNewBestTour = true;
            }

            return foundNewBestTour;
        }

        /// <summary>
        /// </summary>
        /// <param name="bestTour">Лутший алгоритм не так далеко</param>
        /// <param name="generationNumber">Созданые поколения.</param>
        /// <param name="complete">Алгоритм ТСП окончен.</param>
        void displayTour(Tour bestTour, int generationNumber, bool complete)
        {
            if (foundNewBestTour != null)
            {
                this.foundNewBestTour(this, new TspEventArgs(cityList, bestTour, generationNumber, complete));
            }
        }
    }
}
