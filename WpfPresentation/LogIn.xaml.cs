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
using LogicLayer;
using DataObjects;
using LogicLayerInterfaces;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private Employee _user = null;

        public LogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            IEmployeeManager employeeManager = new EmployeeManager();

            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (email.Length < 6)
            {
                MessageBox.Show("Invalid email address.");
                txtEmail.Text = "";
                txtEmail.Focus();
                return;
            }
            if (password == "")
            {
                MessageBox.Show("You must enter a password.");
                txtPassword.Focus();
                return;
            }

            try
            {
                _user = employeeManager.LoginUser(email, password);
                MessageBox.Show("Welcome " + _user.GivenName + "\n" +
                    "You are logged in as " + _user.Roles[0]);
                MainWindow mainWindow = new MainWindow(_user);
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
    }
}
