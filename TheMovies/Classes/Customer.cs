namespace TheMovies
{
    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Customer(string name, string email, string phone)  {
            Name = name;
            Email = email;
            PhoneNumber = phone;
        }

        public override string ToString() {
            return $"{Name};{Email};{PhoneNumber}";
        }
    }
}
