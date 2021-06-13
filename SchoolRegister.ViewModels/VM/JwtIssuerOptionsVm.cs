namespace SchoolRegister.ViewModels.VM {
    public class JwtOptionsVm {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpirationMinutes { get; set; }
    }
}