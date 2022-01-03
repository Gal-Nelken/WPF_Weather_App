using System.Collections.ObjectModel;
using System.ComponentModel;
using WPA_Weather_App.ViewModel.Commands;
using WPA_Wether_App.Model;
using WPA_Wether_App.ViewModel.Helpers;

namespace WPA_Weather_App.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {
        private string query;

        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged("Query");
            }
        }

        private CurrentConditions currentConditions;

        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set
            {
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }

        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetCurrentCondition();
            }
        }

        public ObservableCollection<City> Cities { get; set; }

        public SearchCommand SearchCommand { get; set; }

        // CONSTRUCTOR 
        public WeatherVM()
        {
            Cities = new ObservableCollection<City>();
            SearchCommand = new SearchCommand(this);

            // In design only
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {

                SelectedCity = new City
                {
                    LocalizedName = "Tel-Aviv Yafo"
                };
                CurrentConditions = new CurrentConditions
                {
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = "15"
                        }
                    },
                    WeatherText = "Cloudy"
                };
            }
        }


        public async void MakeQuery()
        {
            try
            {
                var cities = await AccuWeatherHelper.GetCities(Query);

                Cities.Clear();
                foreach (City city in cities) Cities.Add(city);
            }
            catch
            {

            }
        }

        private async void GetCurrentCondition()
        {
            try
            {
                CurrentConditions = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
            }
            catch
            {

            }

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


