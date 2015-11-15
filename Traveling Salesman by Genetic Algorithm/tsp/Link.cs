//ЧДТУ Варварецкий
using System;
using System.Collections.Generic;
using System.Text;

namespace Tsp
{
    /// <summary>
    /// Индивидуальная связь между 2 городами в туре.
    /// Этот город соединяет 2 других города.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Соединение с "Первым" Городом.
        /// </summary>
        private int connection1;
        /// <summary>
        /// Соединение с "Первым" Городом.
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
        /// Соединение с "Вторым" Городом.
        /// </summary>
        private int connection2;
        /// <summary>
        /// Соединение с "Вторым" Городом.
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
