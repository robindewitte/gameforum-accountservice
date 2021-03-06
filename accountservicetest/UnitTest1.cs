using fictivus_accountservice;
using fictivus_accountservice.Controllers;
using fictivus_accountservice.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace accountservicetest
{
    public class UnitTest1
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public Microsoft.Extensions.Configuration.IConfigurationRoot Configuration { get; private set; }

        public UnitTest1()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>().ConfigureAppConfiguration(config =>
               {
                   Configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json")
                     .Build();

                   config.AddConfiguration(Configuration);
               }));
            _client = _server.CreateClient();
        }

        [Theory]
        [InlineData("ikdoemaarwat")]
        [InlineData("ikdoemaarwat@ergens,net")]
        [InlineData("ikdoemaarwatergens.net")]
        public void ValidateEmail_WrongEmailConventions_ReturnFalse(string value)
        {
            //arrange above
            //act
            bool result = AccountController.ValidateEmail(value);

            //assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("ikdoemaarwat@ergens.net")]
        [InlineData("T0o$hort")]
        [InlineData("longenoughnosigns")]
        [InlineData("Longenoughnosigns")]
        [InlineData("L0ngenoughnosigns")]
        public void ValidatePw_WrongPwConventions_ReturnFalse(string value)
        {
            //arrange above
            //act
            bool result = AccountController.ValidatePassword(value);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateEmail_RightEmailConventions_ReturnTrue()
        {
            //arrange and act
            bool result = AccountController.ValidateEmail("ikdoemaarwat@ergens.net");

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePw_RightPwConventions_ReturnTrue()
        {
            //arrange and act
            bool result = AccountController.ValidatePassword("L0ngenoughw!thsigns");

            //assert
            Assert.True(result);
        }


        //does not work when service is not running
        /*
        [Fact]
        public async Task ValidateLogin_WrongCredentials_False()
        {
            //arrange
            LoginDTO loginDTO = new LoginDTO("robintest", "ditmoetfalen");
            var loginDTOstring = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");

            //act
            var response = await _client.PostAsync("https://localhost:5003/api/user/login", loginDTOstring);
            var responseString = await response.Content.ReadAsStringAsync();
            //assert
            Assert.Equal("Verkeerd", responseString);
        }

        */

/*
 does not work when service is not running
        [Fact]
        public async Task ValidateLogin_RightCredentials_True()
        {
            //arrange
            LoginDTO loginDTO = new LoginDTO("robintest", "V!rkeerd1234");
            var loginDTOstring = new StringContent(JsonConvert.SerializeObject(loginDTO), Encoding.UTF8, "application/json");
            //act
            var response = await _client.PostAsync("https://localhost:5003/api/user/login", loginDTOstring);
            var responseString = await response.Content.ReadAsStringAsync();
            //assert
            Assert.NotEqual("verkeerd", responseString);
        }
*/
    }
}
