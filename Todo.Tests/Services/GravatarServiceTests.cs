using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Services;
using Moq;
using System.Net.Http;
using Xunit;
using FluentAssertions;
using System.Net.Mail;

namespace Todo.Tests.Services
{
    public class GravatarServiceTests
    {
        private readonly IGravatarService _sut;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        
        public GravatarServiceTests() 
        {
            //TODO:replace below with wiremock address and set up wiremock to return the correct response in each test
            _httpClientFactory.Setup(m => m.CreateClient("Gravatar")).Returns(new HttpClient() { BaseAddress = new Uri("https://gravatar.com") });
            _sut = new GravatarService(_httpClientFactory.Object);
        }

        [Fact]
        public async Task GetProfileEmailAddressHasProfile()
        {
            var emailAddress = "beau@automattic.com";
;

            var result = await _sut.GetProfile(emailAddress);

            result.Should().NotBeNull();

            result.Name.Should().Be("Beau Lebens");
            result.Photos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetProfileEmailAddressDonestHaveProfile()
        {
            var emailAddress = "bobbybobbob@smyth.com";            

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetProfile(emailAddress));
        }

        [Fact]
        public async Task GetProfilesEmailAddressesHaveProfile()
        {
            var emailAddresses = new string[2] { "beau@automattic.com", "dave@stormideas.com" };

            var result = await _sut.GetProfiles(emailAddresses);

            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }
    }
}
