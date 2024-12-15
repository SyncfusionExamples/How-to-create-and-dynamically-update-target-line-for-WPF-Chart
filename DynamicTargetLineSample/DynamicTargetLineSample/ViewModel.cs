using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DynamicTargetLineSample
{
    internal class ViewModel:INotifyPropertyChanged
    {
        private double y1;
        public double Y1
        {
            get => y1;
            set
            {
                if(y1 != value)
                {
                    y1 = value;
                    OnPropertyChanged(nameof(Y1));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<Model> Data { get; set; }

        public ViewModel()
        {
            Y1 = 12000;
            Data = new ObservableCollection<Model>()
            {
                new Model() { Months = "January", Revenue = 10000 },
                new Model() { Months = "February", Revenue = 13500 },
                new Model() { Months = "March", Revenue = 16000 },
                new Model() { Months = "April", Revenue = 14000 },
                new Model() { Months = "May", Revenue = 12500 },
                new Model() { Months = "June", Revenue = 18000 },
                new Model() { Months = "July", Revenue = 11700 }
            };
        }
    }
}
