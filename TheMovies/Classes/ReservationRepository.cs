using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace TheMovies
{
    public class ReservationRepository
    {
        private List<Reservation> reservations = new List<Reservation>();
        
        public ReservationRepository() 
        {
            InitializeRepository();
        }


        private void InitializeRepository()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("Reservations.txt"))
                {
                    string line = sr.ReadLine();

                    while (line != null)
                    {
                        string[] parts = line.Split(',');

                        string seatInfo = parts[2];

                        string[] seats = seatInfo.Split("|");

                        Customer newCus = new Customer(parts[3], parts[4], parts[5]);

                        Movie stupidMovie = new Movie(parts[8], parts[9], parts[10], parts[11]);

                        Show newShow = new Show(parts[6], DateTime.Parse(parts[7]), stupidMovie);

                        Add(int.Parse(parts[0]), double.Parse(parts[1]), seats, newShow, newCus);

                        line = sr.ReadLine();
                    }
                }
            }
            catch (IOException)
            {
                throw;
            }
        }

        public Reservation Add(int amount, double price, string[] seat, Show show, Customer customer)
        {
            Reservation result = null;

            if (amount > 0 && price > 0 && seat != null && seat.Length > 0 && show != null && customer != null)
            {
                result = new Reservation(amount, price, seat, show, customer);

                reservations.Add(result);

                Debug.WriteLine(reservations.ToString());
            }
            else
            {
                throw new ArgumentException("Not all arguments are valid");
            }

            return result;
        }


        public void Save(int id) 
        {
            foreach (Reservation item in reservations) 
            {
                using (StreamWriter sw = new StreamWriter("Reservations.txt"))
                {
                    string final = $"{item.Amount},{item.Price},";

                    int seatAmt = item.Seat.Length;

                    string seatString = "";

                    for (int i = 0; i < seatAmt; i++)
                    {
                        seatString += item.Seat[i];

                        if (i < seatAmt - 1)
                        {
                            seatString += "|";
                        }
                    }

                    final += $"{seatString},";

                    string[] cusInfo = item.GetCustomerInfo().Split(";");

                    final += $"{cusInfo[0]},{cusInfo[1]},{cusInfo[2]},";




                }
            }
        }


        public void Remove(int id) { }

        public List<Reservation> GetAll() 
        {
            return reservations;
        }

    }
}
