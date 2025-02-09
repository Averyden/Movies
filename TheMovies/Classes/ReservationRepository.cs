using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TheMovies
{
    public class ReservationRepository
    {
        private readonly string conString;

        public ReservationRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            conString = config.GetConnectionString("Jens");
        }

        public Reservation Add(int amount, double price, string[] seat, Show show, Customer customer)
        {
            Reservation result = null;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string query = "INSERT INTO Reservation (Amount, Price, Seats, ShowId, CustomerId) OUTPUT INSERTED.Id VALUES (@amount, @price, @seats, @show, @customer)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@seats", string.Join(",", seat));
                    cmd.Parameters.AddWithValue("@show", show.Id);
                    cmd.Parameters.AddWithValue("@customer", customer.Id);

                    int newId = (int)cmd.ExecuteScalar();
                    result = new Reservation(amount, price, seat, show, customer) { Id = newId };
                }
            }
            return result;
        }

        public void Remove(int id)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Reservation WHERE Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Reservation> GetAll()
        {
            List<Reservation> reservations = new List<Reservation>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation", con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string[] seatArray = reader["Seat"].ToString().Split(',');
                        Show show = new Show("TBD", DateTime.Now, new Movie("TBD", "TBD", "TBD", "TBD")); // Fetch from DB in real scenario
                        Customer customer = new Customer("TBD", "TBD", "TBD"); // Fetch from DB in real scenario

                        reservations.Add(new Reservation(
                            Convert.ToInt32(reader["Amount"]),
                            Convert.ToDouble(reader["Price"]),
                            seatArray,
                            show,
                            customer
                        )
                        { Id = Convert.ToInt32(reader["Id"]) });
                    }
                }
            }
            return reservations;
        }
    }
}
