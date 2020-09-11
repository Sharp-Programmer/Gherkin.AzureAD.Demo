using System.Runtime.InteropServices.ComTypes;

namespace Gherkin.Catalogue.WebClient.Models
{
    public class Constants
    {
        public const string ScopeUserRead = "User.read";
        public const string ProductClientName = "ProductApiClient";

        public const string AzureAdConfigSectionName = "AzureAd";

        public const string ProductApiConfigSectionName = "ProductApi";

        public const string Bearer = nameof(Bearer);
    }
}