using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    public class User
    {
        #region Constants
        internal int MinUserNameLength = 4;
        #endregion

        #region Fields
        private string name;
        #endregion

        #region Properties
        public User(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        #endregion

        #region Methods
        public bool HasValidName()
        {
            bool isValid = false;

            if (this.Name.Length >= MinUserNameLength)
            {
                isValid = true;
            }

            return isValid;
        }
        #endregion
    }
}
