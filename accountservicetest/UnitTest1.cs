using fictivus_accountservice;
using fictivus_accountservice.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
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
        public void ValidateEmailReturnFalse(string value)
        {
            bool result = AccountController.ValidateEmail(value);

            Assert.False(result);
        }

        [Theory]
        [InlineData("ikdoemaarwat@ergens.net")]
        [InlineData("T0o$hort")]
        [InlineData("longenoughnosigns")]
        [InlineData("Longenoughnosigns")]
        [InlineData("L0ngenoughnosigns")]
        public void ValidatePwReturnFalse(string value)
        {
            bool result = AccountController.ValidatePassword(value);

            Assert.False(result);
        }

        [Fact]
        public void ValidateEmailReturnTrue()
        {
            bool result = AccountController.ValidateEmail("ikdoemaarwat@ergens.net");

            Assert.True(result);
        }

        [Fact]
        public void ValidatePwReturnTrue()
        {
            bool result = AccountController.ValidatePassword("L0ngenoughw!thsigns");

            Assert.True(result);
        }
    }
}
