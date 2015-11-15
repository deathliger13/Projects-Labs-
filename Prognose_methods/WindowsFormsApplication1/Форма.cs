using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Форма : Form
    {
        double[] Data_x;
        double[] Data_y;
        double[] Шуканий_ряд;
        int M = 0, N = 0;
        int L = 3;
        double al = 0.1;

        public Форма()
        {
            InitializeComponent();
        }


        private void Обробник_для_кнопки_завантаження_даних(object sender, EventArgs e)
        {
            label2.Text = "";
            label5.Text = "";

            openFileDialog1.InitialDirectory = "c:";
            openFileDialog1.Filter = "CSV files (*.csv)|*.CSV";
            openFileDialog1.FilterIndex = 2;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    chart1.Series[0].Points.Clear();
                    chart1.Series[1].Points.Clear();
                    chart1.Series[2].Points.Clear();
                    string FileName = openFileDialog1.FileName;
                    DataTable dt = new DataTable("Data");
                    using (OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" +
                       Path.GetDirectoryName(FileName) + "\";Extended Properties='text;HDR=yes;FMT=Delimited(;)';"))
                    {
                        using (OleDbCommand cmd = new OleDbCommand(string.Format("select *from[{0}]", new FileInfo(FileName).Name), cn))
                        {
                            cn.Open();
                            using (OleDbDataAdapter adepter = new OleDbDataAdapter(cmd))
                            {
                                adepter.Fill(dt);
                            }
                        }
                    }                    
                    
                    dataGridView1.DataSource = dt;
                    M = dataGridView1.ColumnCount;
                    N = dataGridView1.RowCount;
                    
                    for (int i=0; i<M;i++)
                        dataGridView1.AutoResizeColumn(i);
                    N--;
                    dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    Функція_отримання_даних_з_таблиці();
                    Візуалізація_результатів("Вхідний ряд", Data_x, Data_y, N);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            }
        }

        private void Візуалізація_результатів(string name_series, double[] x, double[] y, int n)
        {
            for (int i = 0; i < n; i++)
                chart1.Series[name_series].Points.AddXY(x[i], y[i]);
        }

        private void Функція_отримання_даних_з_таблиці()
        {
            Data_x = new double[N];
            Data_y = new double[N];
            
            for (int i = 0; i < N; i++)
            {
                Data_x[i] = Convert.ToDouble(dataGridView1[0, i].Value);
                Data_y[i] = Convert.ToDouble(dataGridView1[1, i].Value);
            }
        }

        private void Обробник_для_кнопки_обрахувати(object sender, EventArgs e)
        {            
            if (N < 3)
                return;
            try
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                chart1.Series[2].Points.Clear();

                Візуалізація_результатів("Вхідний ряд", Data_x, Data_y, N);
                Обробка_згладжуванням Прогнозування = new Обробка_згладжуванням(Data_x, Data_y, N, L);
                Шуканий_ряд = Прогнозування.Функція_згладжування();
                for (int i = 0; i < N - L; i++)
                    chart1.Series["Експ. згладж. ряд"].Points.AddXY(Data_x[i + L], Шуканий_ряд[i]);
                chart1.Series["Експ. згладж. ряд"].Points.AddXY(Data_x[N - 1] + 2, Шуканий_ряд[N - L]);
                label2.Text = Math.Round(Шуканий_ряд[N - L], 2).ToString() + "\n" + ((Шуканий_ряд[N - L-1] + Шуканий_ряд[N - L])/2).ToString();
                label2.BackColor = Color.Red;
                label2.ForeColor = Color.Yellow;
            }
            catch
            {
                MessageBox.Show("Кількість точок для прогнозу більша за загальну кількість точок!");
            }
        }

        private void Обробник_випадаючого_списку(object sender, EventArgs e)
        {
            try
            {
                L = Convert.ToInt32(cB1.SelectedItem);
                Обробник_для_кнопки_обрахувати(sender, e);
            }
            catch
            {
                MessageBox.Show("Кількість точок для прогнозу більша за загальну кількість точок!");
            }
        }

        private void Обробка_виходу_з_програми(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (N < 3)
                return;
            try
            {
                double al = Convert.ToDouble(comboBox1.SelectedItem);
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                chart1.Series[2].Points.Clear();

                Візуалізація_результатів("Вхідний ряд", Data_x, Data_y, N);
                Обробка_експ_згладжуванням Прогнозування = new Обробка_експ_згладжуванням(Data_y, N, al, L);
                Шуканий_ряд = Прогнозування.Функція_згладжування();
                for (int i = 0; i < N - L; i++)
                    chart1.Series["Згладжуючий ряд"].Points.AddXY(Data_x[i + L], Шуканий_ряд[i]);
                chart1.Series["Згладжуючий ряд"].Points.AddXY(Data_x[N - 1] + 2, Шуканий_ряд[N - L]);
                label5.Text = Math.Round(Шуканий_ряд[N - L], 2).ToString();
                label5.BackColor = Color.Red;
                label5.ForeColor = Color.Yellow;
            }
            catch 
            {
                MessageBox.Show("Кількість точок для прогнозу більша за загальну кількість точок!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                L = Convert.ToInt32(cB1.SelectedItem);
                al = Convert.ToDouble(comboBox1.SelectedItem);
                button4_Click(sender, e);
            }
            catch { }
        }

      
    }
}