namespace TheMovies
{
    public class Reservation
    {
        private Show show;
        private Cinema cinema;

        private Customer customer; 

        private static int id = 0;
        public int Amount { get; set; }
        public double Price { get; set; }
        public string[] Seat { get; set; }

        public Reservation(int amount, double price, string[] seat, Show show, Customer customer)
        {
            Amount = amount;
            Price = price;
            Seat = seat;
            this.show = show;
            this.customer = customer;
            id++;
        }

        public double CalculatePrice()
        {
            return Price * Amount;
        }

        public string GetCustomerInfo()
        {
            return customer.ToString();
        }

        public string GetLocation()
        {
            return cinema.Location;
        }
    }
}
