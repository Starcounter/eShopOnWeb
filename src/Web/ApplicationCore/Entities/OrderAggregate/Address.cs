using System;
using System.Collections.Generic;
using Starcounter.Nova;

namespace ApplicationCore.Entities.OrderAggregate
{
//    [Database]
    public class Address // ValueObject
    {
        public virtual String Street { get; private set; }

        public virtual String City { get; private set; }

        public virtual String State { get; private set; }

        public virtual String Country { get; private set; }

        public virtual String ZipCode { get; private set; }

        protected Address() { }

        public Address(string street, string city, string state, string country, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }
    }
}
