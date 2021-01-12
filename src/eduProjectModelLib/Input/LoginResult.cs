namespace eduProjectModel.Input
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }

        //Naknadno dodan property ID u svrhu testiranja. Izbrisati ako pravi problem.
        public string loggedUserId { get; set; }
    }
}
