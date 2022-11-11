namespace AuthenticationAPI
{
    public class RefreshToken
    {
        public string Token { get; set; }=String.Empty;
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
      


    }
}
