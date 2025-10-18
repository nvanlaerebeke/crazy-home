using Plugwise.Api;

namespace Plugwise;
internal static class Program {
    private static void Main(string[] args) {
        Startup.Start(WebApplication.CreateBuilder(args));
    }
}
