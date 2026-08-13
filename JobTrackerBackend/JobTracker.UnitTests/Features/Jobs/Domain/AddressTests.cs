using FluentAssertions;
using JobTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JobTracker.UnitTests.Features.Jobs.Domain
{
    public class AddressTests
    {
        private Address CreateAddress() 
        {
            var address = new Address(
                "123 Main St", "Anytown", "CA", "12345", 37.7749, -122.4194
                );
            return address;
        }

        [Fact]
        public void CompareAddress_ShouldReturnTrue_ForIdentical_Addresses()
        {
            var address1 = new Address(
                "123 Main St", "Anytown", "CA", "12345", 37.7749, -122.4194
                ); 
            var address2 = new Address(
                "123 Main St", "Anytown", "CA", "12345", 37.7749, -122.4194
                ); 

            (address1 == address2).Should().BeTrue();
            address1.Should().Be(address2);
        }

        // --- Inequality ---

        [Theory]
        [InlineData("999 Other St", "Anytown",  "CA", "12345", 37.7749, -122.4194)]
        [InlineData("123 Main St",  "OtherCity", "CA", "12345", 37.7749, -122.4194)]
        [InlineData("123 Main St",  "Anytown",   "NY", "12345", 37.7749, -122.4194)]
        [InlineData("123 Main St",  "Anytown",   "CA", "99999", 37.7749, -122.4194)]
        [InlineData("123 Main St",  "Anytown",   "CA", "12345", 0.0000, -122.4194)]
        [InlineData("123 Main St",  "Anytown",   "CA", "12345", 37.7749,   0.0000)]
        public void CompareAddress_ShouldReturnFalse_WhenAnyFieldDiffers(
            string street, string city, string state, string zipCode, double lat, double lon)
        {
            var address1 = CreateAddress();
            var address2 = new Address(street, city, state, zipCode, lat, lon);

            address1.Should().NotBe(address2);
            (address1 != address2).Should().BeTrue();
            (address1 == address2).Should().BeFalse();
        }

        // --- Hash codes ---

        [Fact]
        public void GetHashCode_ShouldBeEqual_ForIdenticalAddresses()
        {
            var address1 = CreateAddress();
            var address2 = CreateAddress();

            address1.GetHashCode().Should().Be(address2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ShouldDiffer_ForDifferentAddresses()
        {
            var address1 = CreateAddress();
            var address2 = new Address("999 Other St", "Anytown", "CA", "12345", 37.7749, -122.4194);

            address1.GetHashCode().Should().NotBe(address2.GetHashCode());
        }

        // --- Null handling ---

        [Fact]
        public void CompareAddress_ShouldReturnFalse_WhenRightSideIsNull()
        {
            var address = CreateAddress();

            (address == null).Should().BeFalse();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            address.Equals(null).Should().BeFalse();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        [Fact]
        public void CompareAddress_BothNull_ShouldReturnTrue()
        {
            Address? a = null;
            Address? b = null;

            (a == b).Should().BeTrue();
        }

        // --- Constructor validation ---

        [Theory]
        [InlineData("",           "Anytown", "CA", "12345")]
        [InlineData("123 Main St","",        "CA", "12345")]
        [InlineData("123 Main St","Anytown", "",   "12345")]
        [InlineData("123 Main St","Anytown", "CA", "")]
        public void Constructor_ShouldThrow_WhenRequiredFieldIsEmpty(
            string street, string city, string state, string zipCode)
        {
            Action act = () => new Address(street, city, state, zipCode, 0, 0);
            act.Should().Throw<ArgumentException>();
        }
    }
}
