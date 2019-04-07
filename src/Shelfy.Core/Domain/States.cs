namespace Shelfy.Core.Domain
{
    public static class States
    {
        // User email is not verified
        public static string Unverified => "unverified";
        // User email is verified, account is activated
        public static string Active => "active";
        // 
        public static string Locked => "locked";
    }
}