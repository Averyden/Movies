﻿namespace TheMovies
{
    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumebr { get; set; }

        public Customer(string name, string email, string phone)  {
            Name = name;
            Email = email;
            PhoneNumebr = phone;
        }
    }
}
