using System;

namespace JobsMarketplace.API.Entities
{
    public class Customer
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;

        private Customer() { }

        public Customer(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; set; }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("FirstName is required.", nameof(value))
                : value;
        }
        public string LastName
        {
            get => _lastName;
            set => _lastName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("LastName is required.", nameof(value))
                : value;
        }
    }
}
