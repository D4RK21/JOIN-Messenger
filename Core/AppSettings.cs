namespace Core
{
    public class AppSettings
    {
        public string JsonDirectory { get; set; }
        
        public ConnectionStrings ConnectionStrings { get; set; }
        
        public string Email { get; set; }
        
        public string EmailDisplayName { get; set; }
        
        public string Domain { get; set; }

        public SmtpSettings SmtpSettings { get; set; }
    }
}
