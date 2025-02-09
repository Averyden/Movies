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
                        string[] seatArray = reader["Seat"].ToString().Split(',');
                        Show show = new Show("TBD", DateTime.Now, new Movie("TBD", "TBD", "TBD", "TBD"));
                        Customer customer = new Customer("TBD", "TBD", "TBD"); 

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
    }
}
