using System.Net;
using Home.Tests.TestObjects;
using LanguageExt.Common;
using Moq;
using NUnit.Framework;
using PlugwiseControl.Message.Requests;
using PlugwiseControl.Message.Responses;

namespace Home.Tests.Controllers;

internal sealed class MetricsTest : IntegrationTest {
    private const string Plug1 = "000D6F00004BF588";
    private const string Plug2 = "000D6F00004BA1C6";

    [SetUp]
    public void SetUp() {
        var plug1Response = new PowerUsageResponse();
        plug1Response.AddData("");

        var plug2Response = new PowerUsageResponse();
        plug2Response.AddData("");

        ExternalServices.RequestManager.Setup(x =>
                x.Send<PowerUsageResponse>(It.Is<PowerUsageRequest>(r => r.Mac == Plug1)))
            .Returns(new Result<PowerUsageResponse>(plug1Response));

        ExternalServices.RequestManager.Setup(x =>
                x.Send<PowerUsageResponse>(It.Is<PowerUsageRequest>(r => r.Mac == Plug2)))
            .Returns(new Result<PowerUsageResponse>(plug2Response));

        base.SetUp();
    }

    [Test]
    public async Task GetTest() {
        //Arrange

        //Act
        var response = await GetClient().GetAsync(Routes.Metrics.Metric);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //var responseObj = await response.ToObject<Metrics>();
        //Assert.That(responseObj, Is.Not.Null);
    }
}
