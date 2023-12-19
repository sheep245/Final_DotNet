namespace Point_Of_Sales.Helpers
{
    public class Mailer
    {
        private readonly IConfiguration _configuration;
        public Mailer(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void SendEmail(string to, string subject, string content)
        {

        }
    }
}
