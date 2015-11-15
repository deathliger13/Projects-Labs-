// ЧДТУ Варварецкий
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Tsp
{
    /// <summary>
    /// Условия для проведения мероприятий аргументы, когда класс ТСП хочет графический интерфейс, чтобы привлечь тур.
    /// </summary>
    public class TspEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор по умолшчанию
        /// </summary>
        public TspEventArgs()
        {
        }

        /// <summary>
        /// Конструктор который задает параметры
        /// </summary>
        /// <param name="cityList"></param>
        /// <param name="bestTour"></param>
        /// <param name="generation"></param>
        /// <param name="complete"></param>
        public TspEventArgs(Cities cityList, Tour bestTour, int generation, bool complete)
        {
            this.cityList = cityList;
            this.bestTour = bestTour;
            this.generation = generation;
            this.complete = complete;
        }

        private Cities cityList;
        public Cities CityList
        {
            get
            {
                return cityList;
            }
        }

        private Tour bestTour;
        public Tour BestTour
        {
            get
            {
                return bestTour;
            }
        }

        private int generation;
        public int Generation
        {
            get
            {
                return generation;
            }
            set
            {
                generation = value;
            }
        }

        private bool complete = false;
        public bool Complete
        {
            get
            {
                return complete;
            }
            set
            {
                complete = value;
            }
        }
    }
}