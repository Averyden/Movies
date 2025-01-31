using System.Collections.ObjectModel;
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

        public ObservableCollection<ReservationViewModel> ReserveVM { get; set; }
        public ICommand AddCommand { get; }
        public ICommand DelCommand { get; }
        public ICommand ShowCommand { get; }

        private int _amount; 
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

        // Clever hack but it does not look pretty
        // Find a fix please.
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



        public ReservationViewModel SelectedReserve
        {
            get => _selectedReserve;
            set
            {
                if (_selectedReserve != value)
                {
                    _selectedReserve = value;
                    OnPropertyChanged(nameof(SelectedReserve));
                }
            }
        }

        public MainViewModel() 
        {
            AddCommand = new CommandHandler(OnAddClicked);

            //DelCommand = new CommandHandler();

            ShowCommand = new CommandHandler(OnShowClicked);

            ReserveVM = new ObservableCollection<ReservationViewModel>();

            foreach (Reservation reservation in reserveRepo.GetAll())
            {
                ReservationViewModel newVM = new ReservationViewModel(reservation);
                ReserveVM.Add(newVM);
            }
        }

        private void OnDelClicked()
        {

        }

        private void OnShowClicked()
        {
            if (_selectedReserve != null)
            {
                MessageBox.Show($"Kunde Email: {SelectedReserve.CusMail}\nKunde Navn: {SelectedReserve.CusName}\nMængde Biletter for reservation: {SelectedReserve.Amount}\nBiograf for reservation: {Cinema}\nBiograf lokation: {CinemaLocation}\nDato for reservation: {ShowDate.ToShortDateString()}\nTidspunkt for forstilling: {StartTime}", "Information for reservation");

            } else
            {
                MessageBox.Show("Der er ikke valgt nogen reservation at kigge på", "Fejl");
            }

        }

        private void OnAddClicked()
        {
            double fullPrice = 50 * _amount;
            Movie movie = new Movie("fuck", "shit", "72", "Christopher Nolan");
            Show show = new Show(StartTime, ShowDate, movie); // TODO: find a way to databind the fucken movie.

            Customer cus = new Customer("John", Email, Phone);

            string[] seats = new string[] { "72", "23" };

            Reservation reserve = new Reservation(_amount, fullPrice, seats, show, cus);

            ReservationViewModel newVM = new ReservationViewModel(reserve);
            ReserveVM.Add(newVM);

            SelectedReserve = newVM;

            reserveRepo.Add(_amount, fullPrice, seats, show, cus);
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
