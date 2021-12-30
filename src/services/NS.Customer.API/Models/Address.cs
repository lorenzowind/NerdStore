using NS.Core.DomainObjects;
using System;

namespace NS.Customer.API.Models
{
    public class Address : Entity
    {
        public string PublicArea { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string District { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public Guid CustomerId { get; private set; }

        public CustomerPerson Customer { get; protected set; }

        public Address(string publicArea, string number, string complement, string district, string zipCode, string city, string state)
        {
            PublicArea = publicArea;
            Number = number;
            Complement = complement;
            District = district;
            ZipCode = zipCode;
            City = city;
            State = state;
        }
    }
}
