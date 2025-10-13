using Home.Api;

namespace Home;
public partial class Program {
    private static void Main(string[] args) {
        Startup.Start(WebApplication.CreateBuilder(args));
    }
}
