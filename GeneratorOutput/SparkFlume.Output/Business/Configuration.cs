namespace SparkFlume.Output.Business
{
    public class Configuration
    {
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int MinutesToInclude { get; set; }
        public int TopAmount { get; set; }
        public int CheckInterval { get; set; }
        public int SecondsToWait { get; set; }
    }
}
