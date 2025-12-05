namespace Home.CommandLine;

public interface ICommandLineEntryPoint {
    Task StartAsync(string[] args);
}
