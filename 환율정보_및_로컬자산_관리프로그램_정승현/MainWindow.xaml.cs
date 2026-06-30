using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace 환율정보_및_로컬자산_관리프로그램_정승현
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 실행 전 본인 PC의 서버 이름으로 변경 필요 (예: Server=YOUR_SERVER_NAME\\SQLEXPRESS)
        string con = "Server=YOUR_SERVER_NAME\\SQLEXPRESS;Database=WB43;Trusted_Connection=True;TrustServerCertificate=True";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                string json = await client.GetStringAsync("https://open.er-api.com/v6/latest/USD");

                ExchangeResponse data = JsonConvert.DeserializeObject<ExchangeResponse>(json);

                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();

                    string sql = "INSERT INTO ExchangeRates(QueryTime, Currency, Rate) VALUES(@time,@currency,@rate)";
                    string[] currency = { "KRW", "JPY", "EUR" };
                    double[] rate = { data.rates["KRW"], data.rates["JPY"], data.rates["EUR"] };

                    for (int i = 0; i < 3; i++)
                    {
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                        cmd.Parameters.AddWithValue("@currency", currency[i]);
                        cmd.Parameters.AddWithValue("@rate", rate[i]);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("저장 완료");
            }
            catch
            {
                MessageBox.Show("실패");
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(con);
                conn.Open();

                string sql = "SELECT * FROM ExchangeRates";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                datagrid.ItemsSource = dt.DefaultView;

                conn.Close();
                MessageBox.Show("성공");
            }
            catch
            {
                MessageBox.Show("실패");
            }
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double money = Convert.ToDouble(textBox1.Text);
                SqlConnection conn = new SqlConnection(con);
                conn.Open();

                string sql = "SELECT TOP 1 Rate FROM ExchangeRates WHERE Currency='KRW' ORDER BY Id DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                double rate = Convert.ToDouble(cmd.ExecuteScalar());
                double usd = money / rate;
                textBox2.Text = usd.ToString("F2") + " USD";

                conn.Close();
                MessageBox.Show("성공");
            }
            catch
            {
                MessageBox.Show("실패");
            }
        }
    }
}
