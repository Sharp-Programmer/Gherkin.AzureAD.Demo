using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Gherkin.Catalogue.WebClient.NetFramework.Models;

using Gherkin.Catalogue.WebClient.NetFramework.Utils;

using Microsoft.Identity.Client;

using Newtonsoft.Json;

namespace Gherkin.Catalogue.WebClient.NetFramework.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            IConfidentialClientApplication app = MsalAppBuilder.BuildConfidentialClientApplication();
            AuthenticationResult result = null;

            var accounts = await app.GetAccountsAsync();

            string[] scopes = { ProductApiConfiguration.ViewScope };

            try
            {
                // try to get token silently from the token cache
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync().ConfigureAwait(false);
            }
            catch (MsalUiRequiredException)
            {
                ViewBag.Relogin = "true";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error has occurred. Details: " + ex.Message;
                return View();
            }
            var products = new List<Product>();
            if (result != null)
            {
                // Use the token to read email
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
                HttpResponseMessage responseMessage = await httpClient.GetAsync(ProductApiConfiguration.GetProductsUrl);

                string productsResult = await responseMessage.Content.ReadAsStringAsync();
                
                if(productsResult.Length > 0)
                {
                    products = JsonConvert.DeserializeObject<List<Product>>(productsResult);
                }
                ViewBag.Message = productsResult;
            }
            

            return View(products);
        }
    }
}