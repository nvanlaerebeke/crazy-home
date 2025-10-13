using NUnit.Framework;

namespace Home.Tests.TestObjects;

public abstract class IntegrationTest {
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void OneTimeSetup() {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient(new () { AllowAutoRedirect = false });
    }

    [OneTimeTearDown]
    public void OneTimeTeardown() {
        _client.Dispose();
        _factory.Dispose();
    }
    
    protected HttpClient GetClient() => _client;
}
