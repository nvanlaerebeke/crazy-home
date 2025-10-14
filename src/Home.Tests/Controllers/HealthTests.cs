using System.Net;
using Home.Tests.TestObjects;
using NUnit.Framework;

namespace Home.Tests.Controllers;

internal sealed class HealthTests : IntegrationTest {
    [Test]
    public async Task GetAllTests() {
        //Arrange
        
        //Act
        var live = await GetClient().GetAsync(Routes.Health.Health());
        var ready = await GetClient().GetAsync(Routes.Health.Health());
        var health = await GetClient().GetAsync(Routes.Health.Health());
        
        //Assert
        Assert.That(live.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(ready.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(health.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}

