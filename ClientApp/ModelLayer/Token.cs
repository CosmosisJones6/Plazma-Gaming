using Microsoft.Extensions.Configuration;

namespace ClientApp.ModelLayer
{
    internal static class Token
    {
        internal static string Value = new ConfigurationBuilder().AddUserSecrets<Developer>().Build().GetSection("Token").Value;
    }
}
