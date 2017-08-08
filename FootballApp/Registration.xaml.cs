using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;

namespace FootballApp
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private SqlConnection connection;
        public Registration(SqlConnection conn)
        {
            connection = conn;
            InitializeComponent();
        }

        private void ClearTextBoxes()
        {
            Login.Clear();
            Password.Clear();
            SecondPassword.Clear();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password != SecondPassword.Password)
            {
                ClearTextBoxes();
                MessageBox.Show(Errors.GetErrorById(1), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Password.Password == "" || Login.Text == "")
            {
                ClearTextBoxes();
                MessageBox.Show(Errors.GetErrorById(0), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT into FootballDb.dbo.Users values ('" + Login.Text + "','" + Password.Password + "')",connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
                new MainWindow().Show();
                this.Close();
            }
        }

        private void Back_OnClick_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
