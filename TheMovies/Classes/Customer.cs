namespace TheMovies
{
    public class Customer
    {
        private static int id = 0;
        public int Id { get { return id; } }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Customer(string name, string email, string phone)  {
            Name = name;
            Email = email;
            PhoneNumber = phone;
            id++;

        }

        public override string ToString() {
            return $"{Name};{Email};{PhoneNumber}";
        }
    }
}
