using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace TheMovies
{
    public class ReservationRepository
    {
        private List<Reservation> reservations = new List<Reservation>();

        //private void InitializeRepository()
        //{
        //    try
        //    {   // Open the text file using a stream reader.
        //        using (StreamReader sr = new StreamReader("Persons.csv"))
        //        {
        //            // Read the stream to a string, and instantiate a Person object
        //            string line = sr.ReadLine();

        //            while (line != null)
        //            {
        //                string[] parts = line.Split(',');

        //                // parts[0] contains first name, parts[1] contains last name, parts[2] contains age as text, parts[3] contains phone

        //                Add(parts[0], parts[1], int.Parse(parts[2]), parts[3]);

        //                //Read the next line
        //                line = sr.ReadLine();
        //            }
        //        }
        //    }
        //    catch (IOException)
        //    {
        //        throw;
        //    }
        //}

        public void Add(Reservation reservation)
        {
            reservations.Add(reservation);
            Debug.WriteLine(reservations.ToString());
        }

        public void Save(int id) 
        {
            
        }


        public void Remove(int id) { }

        public List<Reservation> GetAll() 
        {
            return reservations;
        }

    }
}
