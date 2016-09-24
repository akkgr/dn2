namespace cinnamon.api
{
    public class AppOptions
    {
        public Error Error { get; set; }
        public DataConnection DataConnection { get; set; }

    }

    public class DataConnection
    {
        public string Connection { get; set; }
        public string Database { get; set; }
    }

    public class Error
    {
        public string CustomerHasRepairs { get; set; }
        public string ProductHasRepairs { get; set; }
        public string ProductCategoryHasDocuments { get; set; }
        public string RepairDelete { get; set; }
        public string TypeHasRequests {get; set;}
    }
}