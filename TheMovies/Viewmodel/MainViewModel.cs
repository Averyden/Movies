using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace TheMovies
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ReservationRepository reserveRepo = new ReservationRepository();
        private ReservationViewModel _selectedReserve;
        public ICommand AddCommand { get; }

        private string _startTime;
        private string _phone;
        private string _email;
        private DateTime _showDate = DateTime.Now; 
        private string _movie; 
        private string _cinema;
        private string _cinemaLocation;


        public string StartTime
        {
            get { return _startTime; } 
            set { _startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public DateTime ShowDate
        {
            get { return _showDate; }
            set { _showDate = value; OnPropertyChanged(nameof(ShowDate)); }
        }

        public string Movie
        {
            get { return _movie; }
            set
            {
                string[] fullValue = value.Split(' ');
                _movie = fullValue[1];
                OnPropertyChanged(nameof(Movie));
            }
        }

        // TODO: ask leif about type consistency
        private int _amount; 
        public string Amount
        {
            get { return _amount.ToString(); }
            set
            {
                string[] fullValue = value.Split(' ');
                _amount = int.Parse(fullValue[1]);
                OnPropertyChanged(nameof(Amount));
            }
        }

        public string Cinema 
        {
            get { return _cinema; }
            set 
            {
                string[] fullValue = value.Split(' ');
                _cinema = fullValue[1];
                OnPropertyChanged(nameof(Cinema));
            }
        }



        public string CinemaLocation
        {
            get { return _cinemaLocation; }
            set 
            { 
                string[] fullValue = value.Split(' ');
                _cinemaLocation = fullValue[1];
                OnPropertyChanged(nameof(CinemaLocation)); 
            }
        }


        public MainViewModel() 
        {
            AddCommand = new CommandHandler(OnAddClicked);
        }

        private void OnAddClicked()
        {
            double fullPrice = 50 * _amount;
            Movie movie = new Movie("fuck", "shit", "72", "Christopher Nolan");
            Show show = new Show(StartTime, ShowDate, movie); // TODO: find a way to databind the fucken movie.

            Customer cus = new Customer("John", Email, Phone);

            string[] seats = new string[] { "72", "23" };

            Reservation reserve = new Reservation(_amount, fullPrice, seats, show, cus);

            reserveRepo.Add(reserve);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)

        {
            var value = this.GetType().GetProperty(propertyName)?.GetValue(this, null);

            Debug.WriteLine($"Property: {propertyName}, Value: {value}");


            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)

            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }

    }
}
