using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Model
{
    [Serializable]
    public class Appointment : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal DateTime startDate { get; set; }
        internal DateTime endDate { get; set; }
        internal string title { get; set; }
        internal string description { get; set; }
    }

}
