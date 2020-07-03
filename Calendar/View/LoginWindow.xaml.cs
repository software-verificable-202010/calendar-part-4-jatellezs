using Calendar.Model;
using CalendarProject.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace Calendar.View
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region Constants
        internal string ErrorMessage = "Username must be 4 characters minimum";
        internal string MessageTitle = "Calendar";
        internal string PathToUsersFile = "Users.txt";
        #endregion

        #region Fields
        private readonly UserDatabase userDatabase;
        #endregion

        #region Methods
        public LoginWindow()
        {
            InitializeComponent();

            userDatabase = Utils.DeserializeUsersFile(PathToUsersFile);
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = TextBoxUserName.Text;
            User user = new User(userName);

            if (user.HasValidName())
            {
                userDatabase.SaveUser(user);
                CreateAndDisplayMainWindow(userName);
                this.Close();
            }
            else
            {
                DisplayMessageBoxError();
            }
        }

        private static void CreateAndDisplayMainWindow(string userName)
        {
            MainWindow mainWindow = new MainWindow(DateTime.Now, new User(userName));
            mainWindow.Show();
        }

        private void DisplayMessageBoxError()
        {
            MessageBox.Show(ErrorMessage, MessageTitle);
        }
        #endregion
    }
}
