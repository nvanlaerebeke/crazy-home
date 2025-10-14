using Home.Api;

namespace Home;
public partial class Program {
    private static void Main(string[] args) {
        new Startup().Start(WebApplication.CreateBuilder(args));
    }
}
