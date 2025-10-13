namespace Home.Config; 

public class SettingsProvider {
    public ISettings Get() {
        return new Settings();
    }
}
