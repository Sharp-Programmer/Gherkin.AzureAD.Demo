using System.Configuration;
using System.Threading.Tasks;

namespace Gherkin.Catalogue.WebClient.NetFramework.Utils
{
    public static class ProductApiConfiguration
    {
        public static string GetProductsUrl { get; } = ConfigurationManager.AppSettings["productApi:Url"];

        public static string ViewScope { get; set; } = ConfigurationManager.AppSettings["productApi:ViewScope"];
    }
}
