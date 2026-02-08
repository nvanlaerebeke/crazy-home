namespace Home.Config;

public class Power {
    public Uri? MetricsUri {
        get {
            var envVar = Environment.GetEnvironmentVariable("POWER_URL");
            return !string.IsNullOrEmpty(envVar) ? new Uri($"{envVar.Trim("/")}/metrics") : null;
        }
    }
}
