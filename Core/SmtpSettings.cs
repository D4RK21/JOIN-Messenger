namespace Core
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        
        public bool EnableSsl { get; set; }
        
        public bool UseDefaultCredentials { get; set; }
        
    }
}
