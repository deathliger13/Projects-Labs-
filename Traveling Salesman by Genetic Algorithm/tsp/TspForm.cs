//��� �������, ��� ������
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Globalization;

namespace Tsp
{
    /// <summary>
    /// ������� ����� ��� ������ ������������
    /// </summary>
    public partial class TspForm : Form
    {
        /// <summary>
        /// </summary>
        Cities cityList = new Cities();

        /// <summary>
        /// </summary>
        Tsp tsp;

        /// <summary>
        /// </summary>
        Image cityImage;

        /// <summary>
        /// </summary>
        Graphics cityGraphics;

        /// <summary>
        /// </summary>
        /// <param name="sender">������ ��������� ����� �������.</param>
        /// <param name="e">���������� �������.</param>
        public delegate void DrawEventHandler(Object sender, TspEventArgs e);

        /// <summary>
        /// constructor.
        /// </summary>
        public TspForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��� �������� ������ �������, ��� ������ ����� ������ ���.
        /// �� ������ ������� ����������� �� GUI ������, ������ ��� ������ �����-���� ����� ����.        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsp_foundNewBestTour(object sender, TspEventArgs e)
        {
            if ( this.InvokeRequired )
            {
                try
                {
                    this.Invoke(new DrawEventHandler(DrawTour), new object[] { sender, e });
                    return;
                }
                catch (Exception)
                {
                }
            }

            DrawTour(sender, e);
        }

        /// <summary>
        /// ����� "�����" ��� �� ��������� TSP ��� �������.
        /// ��������� ��� �� �����, � �������� ���� ����� �������.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawTour(object sender, TspEventArgs e)
        {
            this.lastFitnessValue.Text = Math.Round(e.BestTour.Fitness, 2).ToString(CultureInfo.CurrentCulture);
            this.lastIterationValue.Text = e.Generation.ToString(CultureInfo.CurrentCulture);

            if (cityImage == null)
            {
                cityImage = new Bitmap(tourDiagram.Width, tourDiagram.Height);
                cityGraphics = Graphics.FromImage(cityImage);
            }

            int lastCity = 0;
            int nextCity = e.BestTour[0].Connection1;

            cityGraphics.FillRectangle(Brushes.White, 0, 0, cityImage.Width, cityImage.Height);
            foreach( City city in e.CityList )
            {
                // ������ "�������" �����.
                cityGraphics.DrawEllipse(Pens.Black, city.Location.X - 2, city.Location.Y - 2, 5, 5);

                //������ �����
                cityGraphics.DrawLine(Pens.Black, cityList[lastCity].Location, cityList[nextCity].Location);

                if (lastCity != e.BestTour[nextCity].Connection1)
                {
                    lastCity = nextCity;
                    nextCity = e.BestTour[nextCity].Connection1;
                }
                else
                {
                    lastCity = nextCity;
                    nextCity = e.BestTour[nextCity].Connection2;
                }
            }

            this.tourDiagram.Image = cityImage;

            if (e.Complete)
            {
                StartButton.Text = "������";
                StatusLabel.Text = "�� ������ ��� ����������� ���� ̳��� � ������";
                StatusLabel.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// ������ ������ �������.
        /// </summary>
        /// <param name="cityList">������ ������� ��� ���������.</param>
        private void DrawCityList(Cities cityList)
        {
            Image cityImage = new Bitmap(tourDiagram.Width, tourDiagram.Height);
            Graphics graphics = Graphics.FromImage(cityImage);

            foreach (City city in cityList)
            {
                graphics.DrawEllipse(Pens.Black, city.Location.X - 2, city.Location.Y - 2, 5, 5);
            }

            this.tourDiagram.Image = cityImage;

            updateCityCount();
        }

        /// <summary>
        /// ������������ ����� ������ ����, ����� ������ �������� TSP.
        /// ���� �� ��� �������, �� ��� ������ ����� ������� ����, � �� ����� ��������������� ���.
        /// � ��������� ������,  </summary>
        /// <param name="sender">������ ��������� �������</param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (tsp != null)
            {
                tsp.Halt = true;
                return;
            }

            int populationSize = 0;
            int maxGenerations = 0;
            int mutation = 0;
            int groupSize = 0;
            int numberOfCloseCities = 0;
            int chanceUseCloseCity = 0;
            int seed = 0;

            try
            {
                populationSize = Convert.ToInt32(populationSizeTextBox.Text, CultureInfo.CurrentCulture);
                maxGenerations = Convert.ToInt32(maxGenerationTextBox.Text, CultureInfo.CurrentCulture);
                mutation = Convert.ToInt32(mutationTextBox.Text, CultureInfo.CurrentCulture);
                groupSize = Convert.ToInt32(groupSizeTextBox.Text, CultureInfo.CurrentCulture);
                numberOfCloseCities = Convert.ToInt32(NumberCloseCitiesTextBox.Text, CultureInfo.CurrentCulture);
                chanceUseCloseCity = Convert.ToInt32(CloseCityOddsTextBox.Text, CultureInfo.CurrentCulture);
                seed = Convert.ToInt32(randomSeedTextBox.Text, CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }

            if (populationSize <= 0)
            {
                MessageBox.Show("�� ������ ������� ���������� ���������", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if (maxGenerations <= 0)
            {
                MessageBox.Show("�� ������ ������� ����������� ������� �������", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if ((mutation < 0) || (mutation > 100))
            {
                MessageBox.Show("������� ������ ���� �� 0 �� 100.", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if ((groupSize < 2) || ( groupSize > populationSize ))
            {
                MessageBox.Show("�� ������ ������� ����� (�����). ����� �� ������ � ����������� ���������.", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if ((numberOfCloseCities < 3) || (numberOfCloseCities >= cityList.Count))
            {
                MessageBox.Show("ʳ������ ������ ���, ��� ������� ��� ������� �������� ��������� ������ ���� ����� 3 � ����� ���������� ����� ���.", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if ((chanceUseCloseCity < 0) || (chanceUseCloseCity > 95))
            {
                MessageBox.Show("����� ��������� ����� ���� ��� �������� ��������� ��������� �� ���� �� 0% - 95%. ", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if (seed < 0)
            {
                MessageBox.Show("�� ������ ������� ������ ���������� ���������� �����", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if (cityList.Count < 5)
            {
                MessageBox.Show("�� ������ ��� ����������� ���� ̳��� � ������. ", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            this.StartButton.Text = "����";
            ThreadPool.QueueUserWorkItem( new WaitCallback(BeginTsp));
        }

        /// <summary>
        /// ��������� ����� TSP.
        /// ��� ������� ��������� �� ���� ���� �������.  </summary>
        /// <param name="stateInfo">Not used</param>
        private void BeginTsp(Object stateInfo)
        {
            int populationSize = Convert.ToInt32(populationSizeTextBox.Text, CultureInfo.CurrentCulture);
            int maxGenerations = Convert.ToInt32(maxGenerationTextBox.Text, CultureInfo.CurrentCulture); ;
            int mutation = Convert.ToInt32(mutationTextBox.Text, CultureInfo.CurrentCulture);
            int groupSize = Convert.ToInt32(groupSizeTextBox.Text, CultureInfo.CurrentCulture);
            int seed = Convert.ToInt32(randomSeedTextBox.Text, CultureInfo.CurrentCulture);
            int numberOfCloseCities = Convert.ToInt32(NumberCloseCitiesTextBox.Text, CultureInfo.CurrentCulture);
            int chanceUseCloseCity = Convert.ToInt32(CloseCityOddsTextBox.Text, CultureInfo.CurrentCulture);

            cityList.CalculateCityDistances(numberOfCloseCities);

            tsp = new Tsp();
            tsp.foundNewBestTour += new Tsp.NewBestTourEventHandler(tsp_foundNewBestTour);
            tsp.Begin(populationSize, maxGenerations, groupSize, mutation, seed, chanceUseCloseCity, cityList);
            tsp.foundNewBestTour -= new Tsp.NewBestTourEventHandler(tsp_foundNewBestTour);
            tsp = null;
        }

        /// <summary>
        /// ���������������� ����� ������ ������ ������� XML �����.
        /// ���� ������ ��������� TSP �� �����������. </summary>
        /// <param name="sender">������ ��������� ������ �������.</param>
        /// <param name="e"></param>
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpenDialog = new OpenFileDialog();
            fileOpenDialog.Filter = "XML(*.xml)|*.xml";
            fileOpenDialog.InitialDirectory = ".";
            fileOpenDialog.ShowDialog();
            fileNameTextBox.Text = fileOpenDialog.FileName;
        }

        /// <summary>
        /// ������������ ������, ����� ������� </summary>
        /// <param name="sender">������ ��������� ������ �������.</param>
        /// <param name="e"></param>
        private void openCityListButton_Click(object sender, EventArgs e)
        {
            string fileName = "";

            try
            {
                if (tsp != null)
                {
                    StatusLabel.Text = "������� �������� ���� ���� ������ ��������";
                    StatusLabel.ForeColor = Color.Red;
                    return;
                }

                fileName = this.fileNameTextBox.Text;
                
                cityList.OpenCityList(fileName);
                DrawCityList(cityList);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("���� �� ��������: " + fileName, "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("������������ ����", "�������!!!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="sender">������ ��������� ������ �������.</param>
        /// <param name="e"></param>
        private void clearCityListButton_Click(object sender, EventArgs e)
        {
            if (tsp != null)
            {
                StatusLabel.Text = "������� �������� ���� ���� ������ ��������";
                StatusLabel.ForeColor = Color.Red;
                return;
            }

            cityList.Clear();
            this.DrawCityList(cityList);
        }

        /// <summary>
        /// ������������ ����� ����� �� ����� ������.
        /// ���� �� �� �������� �������� TSP,
        /// ���������� ����� ����� �� ����� � �������� ��� � ������ ������.</summary>
        /// <param name="sender">������ ��������� ������ �������.</param>
        /// <param name="e"></param>
        private void tourDiagram_MouseDown(object sender, MouseEventArgs e)
        {
            if (tsp != null)
            {
                StatusLabel.Text = "������� �������� ���� ���� ������ ��������";
                StatusLabel.ForeColor = Color.Red;
                return;
            }

            cityList.Add(new City(e.X, e.Y));
            DrawCityList(cityList);
        }

        /// <summary>
        /// ���������� ������� �� �����.
        /// </summary>
        private void updateCityCount()
        {
            this.NumberCitiesValue.Text = cityList.Count.ToString();
        }
    }
}