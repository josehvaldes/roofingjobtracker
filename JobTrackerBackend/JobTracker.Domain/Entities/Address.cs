using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public sealed class Address : ValueObject
    {
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        private Address() { }

        public Address(string street, string city, string state, string zipCode, double latitude, double longitude)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Street is required.", nameof(street));
            if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));
            if (string.IsNullOrWhiteSpace(state)) throw new ArgumentException("State is required.", nameof(state));
            if (string.IsNullOrWhiteSpace(zipCode)) throw new ArgumentException("ZipCode is required.", nameof(zipCode));

            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return ZipCode;
            yield return Latitude;
            yield return Longitude;
        }
    }
}
