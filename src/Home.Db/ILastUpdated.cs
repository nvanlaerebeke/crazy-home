namespace Home.Db;

public interface IHasLastUpdated {
    DateTime LastUpdated { get; set; }
}
