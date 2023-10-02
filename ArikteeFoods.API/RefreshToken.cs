namespace ArikteeFoods.API
{
    public class RefreshToken
    {
        // create 3 properties for refresh token, created at and expires at in 7 days
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
