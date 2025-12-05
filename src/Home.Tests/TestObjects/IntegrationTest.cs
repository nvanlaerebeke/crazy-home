using NUnit.Framework;

namespace Home.Tests.TestObjects;

internal abstract class IntegrationTest {
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;
    protected ExternalServices ExternalServices => _factory.ExternalServices;

    /// <summary>
    /// Setup per test
    ///
    /// Do the web application setup here so that we have services and mock classes per test
    /// </summary>
    [SetUp]
    public void SetUp() {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient(new() { AllowAutoRedirect = false });
    }

    [TearDown]
    public void OneTimeTeardown() {
        _client.Dispose();
        _factory.Dispose();
    }

    protected HttpClient GetClient() => _client;
}
