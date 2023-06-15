namespace BusinessLogic.Options
{
    public class EmailOptions
    {
        public const string Section = "Email";

        public string Username { get; set; }

        public string Address { get; set; }

        public string SmtpServer { get; set; }

        public string Port { get; set; }

        public string Password { get; set; }
    }
}
