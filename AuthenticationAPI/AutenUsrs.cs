namespace AuthenticationAPI
{
    public class AutenUsrs
    {
        public string UserName { get; set; } = string.Empty;
         public byte[] PasswordHash { get; set; }
         public byte[] PasswordSalt { get; set; }

        public string RefreshToken { get; set; }= string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public DateTime TokenCreated { get; internal set; }
    }
}
