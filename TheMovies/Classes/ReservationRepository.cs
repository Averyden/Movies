using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TheMovies
{
    public class ReservationRepository
    {
        private readonly string conString;
        private List<Reservation> reservations = new List<Reservation>();

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

            if (!CustomerExists(customer.Id))
            {
                customer.SetId(InsertNewCustomer(customer));
            }

            if (!ShowExists(show.Id))
            {
               show.SetId(InsertNewShow(show));
            }

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO RESERVATION (CustomerId, ShowId, Amount, SalesPrice, Seats) " +
                    "VALUES (@customer, @show, @amount, @price, @seats)", con))
                {
                    cmd.Parameters.Add("@customer", SqlDbType.Int).Value = customer.Id;
                    cmd.Parameters.Add("@show", SqlDbType.Int).Value = show.Id;
                    cmd.Parameters.Add("@amount", SqlDbType.Int).Value = amount;
                    cmd.Parameters.Add("@price", SqlDbType.Float).Value = price;
                    cmd.Parameters.Add("@seats", SqlDbType.NVarChar).Value = string.Join(",", seat);

                    cmd.ExecuteNonQuery();

                    result = new Reservation(amount, price, seat, show, customer);
                    reservations.Add(result);
                }
            }

            return result;
        }

        private bool CustomerExists(int customerId)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM CUSTOMER WHERE CustomerId = @customerId", con))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        private int InsertNewCustomer(Customer customer)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO CUSTOMER (Name, Email, Telefonnummer, EmployeeId) " +
                    "VALUES (@name, @email, @phone, @employeeId); SELECT SCOPE_IDENTITY();", con))
                {
                    cmd.Parameters.AddWithValue("@name", customer.Name);
                    cmd.Parameters.AddWithValue("@email", customer.Email);
                    cmd.Parameters.AddWithValue("@phone", customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@employeeId", DBNull.Value);

                    var newId = cmd.ExecuteScalar();
                    return Convert.ToInt32(newId);
                }
            }
        }

        private bool ShowExists(int showId)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM SHOWTABLE WHERE ShowId = @showId", con))
                {
                    cmd.Parameters.AddWithValue("@showId", showId);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        private int InsertNewShow(Show show)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO SHOWTABLE (MovieId, Date) " +
                    "VALUES (@movieId, @date); SELECT SCOPE_IDENTITY();", con))
                {
                    cmd.Parameters.AddWithValue("@movieId", 1);  
                    cmd.Parameters.AddWithValue("@date", show.Date);

                    var newShowId = cmd.ExecuteScalar(); 
                    return Convert.ToInt32(newShowId);
                }
            }
        }

        public void Remove(int id)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Reservation WHERE CustomerId = @id OR ShowId = @id", con)) 
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            reservations.RemoveAll(r => r.Id == id); 
        }

        public List<Reservation> GetAll()
        {
            reservations.Clear();

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Reservation", con))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int showId = Convert.ToInt32(reader["ShowId"]);
                        int customerId = Convert.ToInt32(reader["CustomerId"]);

                        Show show = GetShowById(showId);

                        Customer customer = GetCustomerById(customerId);

                        string[] seatArray = reader["Seats"].ToString().Split(',');

                        reservations.Add(new Reservation(
                            Convert.ToInt32(reader["Amount"]),
                            Convert.ToDouble(reader["SalesPrice"]),
                            seatArray,
                            show,
                            customer
                        ));
                    }
                }
            }

            return reservations;
        }

        private Show GetShowById(int showId)
        {
            Show show = null;

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM SHOWTABLE WHERE ShowId = @showId", con))
                {
                    cmd.Parameters.AddWithValue("@showId", showId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int movieId = Convert.ToInt32(reader["MovieId"]);
                            DateTime showTime = Convert.ToDateTime(reader["Date"]);

                            Movie movie = GetMovieById(movieId);

                            show = new Show(reader["Time"].ToString(), showTime, movie);
                            show.SetId(showId);  
                        }
                    }
                }
            }

            return show;
        }

        private Customer GetCustomerById(int customerId)
        {
            Customer customer = null;

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM CUSTOMER WHERE CustomerId = @customerId", con))
                {
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            string email = reader["Email"].ToString();
                            string phone = reader["Telefonnummer"].ToString();

                            customer = new Customer(name, email, phone);
                            customer.SetId(customerId);  
                        }
                    }
                }
            }

            return customer;
        }

        private Movie GetMovieById(int movieId)
        {
            Movie movie = null;

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM MOVIE WHERE MovieId = @movieId", con))
                {
                    cmd.Parameters.AddWithValue("@movieId", movieId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string title = reader["Title"].ToString();
                            string director = reader["Director"].ToString();
                            string genre = reader["Genre"].ToString();
                            string duration = reader["Duration"].ToString();

                            movie = new Movie(title, genre, duration, director);
                        }
                    }
                }
            }

            return movie;
        }
    }
}


