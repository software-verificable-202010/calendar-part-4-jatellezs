using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    class UserDatabase
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
    }
}
