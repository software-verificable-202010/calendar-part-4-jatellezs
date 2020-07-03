using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    public class UserDatabase
    {
        #region Fields
        private readonly List<User> registeredUsers;
        #endregion

        #region Properties
        public UserDatabase()
        {
            registeredUsers = new List<User>() { };
        }

        public List<User> RegisteredUsers
        {
            get
            {
                return registeredUsers;
            }
        }
        #endregion

        #region Methods
        public void SaveUser(User user)
        {
            if (this.RegisteredUsers.Find(u => u.Name == user.Name) == null)
            {
                this.RegisteredUsers.Add(user);
            }

            string PathToUsersFile = "Users.txt";
            this.Serialize(PathToUsersFile);
        }

        public void Serialize(string pathToFile)
        {
            IFormatter writeFormatter = new BinaryFormatter();
            Stream writeStream = new FileStream(pathToFile, FileMode.Create, FileAccess.Write);

            writeFormatter.Serialize(writeStream, this);
            writeStream.Close();
        }
        #endregion
    }
}
