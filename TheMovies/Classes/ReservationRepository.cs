using System.Diagnostics;
using System.IO;
using System;

namespace TheMovies
{
    public class ReservationRepository
    {
        private List<Reservation> reservations = new List<Reservation>();

        private readonly string filePath = "Reservations.txt";

        public ReservationRepository() 
        {
            if (File.Exists(filePath))
            {
                Debug.WriteLine("ok we good YIPPEE");
            }
            else
            {
                try
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        Debug.WriteLine("File created successfully!");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred while creating the file: {ex.Message}");
                }
            }
            InitializeRepository();
        }


        private void InitializeRepository()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
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

            Save();

            return result;
        }


        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(filePath, false)) 
            {
                foreach (Reservation item in reservations)
                {
                    string final = $"{item.Amount},{item.Price},";

                    string seatString = string.Join("|", item.Seat); 
                    final += $"{seatString},";

                    string[] cusInfo = item.GetCustomerInfo().Split(";");
                    final += $"{cusInfo[0]},{cusInfo[1]},{cusInfo[2]},";

                    string[] showInfo = item.GetShowInfo().Split(";");
                    final += $"{showInfo[0]},{showInfo[1]},";

                    string[] movieInfo = item.GetMovieInfoFromShow().Split(";");
                    final += $"{movieInfo[0]},{movieInfo[1]},{movieInfo[2]},{movieInfo[3]}";

                    sw.WriteLine(final);
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
