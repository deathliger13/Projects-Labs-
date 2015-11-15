// ��� �������
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tsp
{
    class Population : List<Tour>
    {
        /// <summary>
        /// ������ ����� ������� ���� ������� ����� �� ��� ��� ������������ ��������.
        /// </summary>
        private Tour bestTour = null;
        /// <summary>
        /// ������ ��� ������� ����� �� ��� ��� ������������ ��������.
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
        /// ������� ��������� ����� ��������� �����.
        /// </summary>
        /// <param name="populationSize">����� ����� ��� ��������.</param>
        /// <param name="cityList">������ ������� � ����.</param>
        /// <param name="rand">��������� ��������� �����. �� �������� ��������, ��� ��� ���������� ����� ����� �������������.</param>
        /// <param name="chanceToUseCloseCity">����� (�� 100), ��� �����, �������, ��� ��������, ����� ���� ������ ����� �������������� � ����� ������ ����� �����.</param>
        public void CreateRandomPopulation(int populationSize, Cities cityList, Random rand, int chanceToUseCloseCity)
        {
            int firstCity, lastCity, nextCity;

            for (int tourCount = 0; tourCount < populationSize; tourCount++)
            {
                Tour tour = new Tour(cityList.Count);

                // �������� ��������� ��������� ����� ��� ����� ����
                firstCity = rand.Next(cityList.Count);
                lastCity = firstCity;

                for (int city = 0; city < cityList.Count - 1; city++)
                {
                    do
                    {
                        // ������ ����� ��������� ������� ��� ���������� ������, ���� �� �� ������ ���,� ������� �� �� ����.
                        if ((rand.Next(100) < chanceToUseCloseCity) && ( cityList[city].CloseCities.Count > 0 ))
                        {
                            // 75% ���� ����� ������� �����, ������� ������ � �����
                            nextCity = cityList[city].CloseCities[rand.Next(cityList[city].CloseCities.Count)];
                        }
                        else
                        {
                            // � ��������� ������, �������� ����� ��������� ���������.
                            nextCity = rand.Next(cityList.Count);
                        }
                        //���������, ��� ��� ����� �� ����, � ���������, ��� ��� �� ���, ��� �� ������.
                    } while ((tour[nextCity].Connection2 != -1) || (nextCity == lastCity));

                    // ����� ��� � ������ A � B, [1] �� A = B � [1] �� ����� B = A
                    tour[lastCity].Connection2 = nextCity;
                    tour[nextCity].Connection1 = lastCity;
                    lastCity = nextCity;
                }

                // ��������� ��� ��������� ������
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
