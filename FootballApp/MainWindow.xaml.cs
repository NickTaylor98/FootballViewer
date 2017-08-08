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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace FootballApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
   public partial class MainWindow : Window
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        //public static extern short GetKeyState(int keyCode);
        SqlConnection connection = new SqlConnection(@"Data Source =.;Initial Catalog = FootballDb;Integrated Security = True;");

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            CheckCapsLock();
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            new Registration(connection).Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "" || Login.Text == "")
            {
                Login.Clear();
                Password.Clear();
                MessageBox.Show(Errors.GetErrorById(0), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                connection.Open();
                using (SqlCommand sqlCommand =
                    new SqlCommand("SELECT Password FROM FootballDb.dbo.Users Where Login='" + Login.Text + "'",
                        connection))
                {
                    SqlDataReader password = sqlCommand.ExecuteReader();
                    if (password.HasRows)
                    {
                        while (password.Read())
                            if (Password.Password == (string)password.GetValue(0))
                            //TODO: link to new window
                            {
                                if (ConnectionToInternet.Connect())
                                {
                                    new Main(Login.Text).Show();
                                    this.Close();
                                }
                                else MessageBox.Show(Errors.GetErrorById(4), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            //MessageBox.Show("Успех!");
                            else
                                MessageBox.Show(Errors.GetErrorById(3), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else MessageBox.Show(Errors.GetErrorById(2), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    password.Close();
                }
                connection.Close();
            }
        }
        private void CheckCapsLock()
        {
            if (Console.CapsLock)
                Capslock.Content = "Caps Lock включен";
            else Capslock.Content = "";
        }
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Button_Click(sender, e);
            CheckCapsLock();
        }
        
    }
}
