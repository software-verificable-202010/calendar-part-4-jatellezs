using Calendar.Model;
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
        internal int MinUserNameLength = 4;
        internal string ErrorMessage = "Username must be 4 characters minimum";
        internal string MessageTitle = "Calendar";
        internal string PathToUsersFile = "Users.txt";
        #endregion

        #region Fields
        private UserDatabase userDatabase;
        #endregion

        #region Methods
        public LoginWindow()
        {
            InitializeComponent();

            DeserializeUsersFile();
        }

        private void DeserializeUsersFile()
        {
            IFormatter readFormatter = new BinaryFormatter();
            Stream readStream = new FileStream(PathToUsersFile, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                userDatabase = readFormatter.Deserialize(readStream) as UserDatabase;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is SerializationException)
            {
                userDatabase = new UserDatabase();
            }

            readStream.Close();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = TextBoxUserName.Text;

            if (IsValidUserName(userName))
            {
                SaveUserInDatabase(new User(userName));
                CreateAndDisplayMainWindow(userName);
                this.Close();
            }
            else
            {
                DisplayMessageBoxError();
            }
        }

        private bool IsValidUserName(string userName)
        {
            bool isValid = false;

            if (userName.Length >= MinUserNameLength)
            {
                isValid = true;
            }

            return isValid;
        }

        private void SaveUserInDatabase(User user)
        {
            if (userDatabase.RegisteredUsers.Find(u => u.Name == user.Name) == null)
            {
                userDatabase.RegisteredUsers.Add(user);
            }

            SerializeUsersFile(userDatabase);
        }

        private void SerializeUsersFile(UserDatabase users)
        {
            IFormatter writeFormatter = new BinaryFormatter();
            Stream writeStream = new FileStream(PathToUsersFile, FileMode.Create, FileAccess.Write);

            writeFormatter.Serialize(writeStream, users);
            writeStream.Close();
        }

        private void CreateAndDisplayMainWindow(string userName)
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
