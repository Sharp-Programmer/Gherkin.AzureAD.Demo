using System.Web;
using System.Web.Mvc;

namespace Gherkin.Catalogue.WebClient.NetFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
