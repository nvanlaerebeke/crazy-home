namespace Home.Tests.TestObjects.Route;

internal sealed class PlugRoutes {
    public string GetAll => "/Plug";
    public string Initialize => "/Plug/Initialize";
    public string Get(string mac) => $"/Plug/{mac}";
    public string On(string mac) => $"/Plug/{mac}/On";
    public string Off(string mac) => $"/Plug/{mac}/Off";
    public string Usage(string mac) => $"/Plug/{mac}/Usage";
    public string Calibrate(string mac) => $"/Plug/{mac}/Calibrate";
    public string SetDate(string mac, long unixDStamp) => $"/Plug/{mac}/SetDateTime/{unixDStamp}";
}

