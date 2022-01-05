using API.DataAccess.Models;
using API.Services.Mappers;
using API.Services.Requests;
using NUnit.Framework;

namespace API.Tests
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void When_MapFrom_Expect_User()
        {
            var testAddress = new AddressRequest
            {
                Country = "TestCountry",
                City = "TestCity",
                StreetName = "TestStreetName",
                StreetNumber = "123",
            };
            var request = new RegisterUserRequest
            {
                FirstName = "testFirstName",
                LastName = "testLastName",
                Email = "testmail@test.com",
                Password = "testPass1!",
                ConfirmPassword = "testPass1!",
                PhoneNumber = "0895488533",
                Address = testAddress
            };

            var result = Mapper.MapFrom(request);
            
            Assert.That(result,Is.Not.Null);
            Assert.That(result,Is.TypeOf<User>());
            Assert.AreSame(result.FirstName,request.FirstName);
            Assert.AreSame(result.Lastname,request.LastName);
            Assert.AreSame(result.Email,request.Email);
            Assert.AreSame(result.PhoneNumber,request.PhoneNumber);
            Assert.AreSame(result.Address.Country,testAddress.Country);
            Assert.AreSame(result.Address.City,testAddress.City);
            Assert.AreSame(result.Address.StreetName,testAddress.StreetName);
            Assert.AreSame(result.Address.StreetNumber,testAddress.StreetNumber);
        }
    }
}
