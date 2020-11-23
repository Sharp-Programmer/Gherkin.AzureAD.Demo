using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gherkin.Catalogue.WebClient.Models;
using Gherkin.Catalogue.Api.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Gherkin.Catalogue.Core;
using static Gherkin.Catalogue.Core.Constants;

namespace Gherkin.Catalogue.WebClient.Controllers
{
    [Route("Products")]
    public class ProudctsController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IDownstreamWebApi _downstreamWebApi;
        private readonly HttpClient _httpClient;
        private readonly AzureAdSettings _azureAdSettings;
        private readonly ProductApiSettings _productApiSettings;

        public ProudctsController(ITokenAcquisition tokAcquisition,
                                    IHttpClientFactory httpClientFactory,
                                    IOptionsMonitor<AzureAdSettings> azureAdSettingsOptionsMonitor,
                                    IOptionsMonitor<ProductApiSettings> productOptionsMonitor)
        {
            _tokenAcquisition = tokAcquisition;
            _httpClient = httpClientFactory.CreateClient(ProductClientName);
            _azureAdSettings = azureAdSettingsOptionsMonitor.CurrentValue;
            _productApiSettings = productOptionsMonitor.CurrentValue;
        }

        [HttpGet]
        [AuthorizeForScopes(ScopeKeySection = "ProductApiScopes")]
        public async Task<IActionResult> Index()
        {
            var accessToken = await GetAccessTokenForProductsApi();

            if (string.IsNullOrEmpty(accessToken))
            {
                return new JsonErrorResult((int)HttpStatusCode.Unauthorized, new ErrorResult("Acquire token interactively"));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, accessToken);
            var productsResponse = await _httpClient.GetAsync(_productApiSettings.Url);

            if (productsResponse.IsSuccessStatusCode)
            {
                var content = await productsResponse.Content.ReadAsStringAsync();

                var produtcts = JsonConvert.DeserializeObject<List<ProductViewModel>>(content);
                return View(produtcts);
            }

            return new RedirectToActionResult("Error", "Home", null);
        }

        private async Task<string> GetAccessTokenForProductsApi()
        {
            var accessToken = string.Empty;
            try
            {
                accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_productApiSettings.Scopes, _azureAdSettings.TenantId);
            }
            catch (MsalUiRequiredException ex)
            {
                await _tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(_productApiSettings.Scopes, ex);
            }
            catch (MicrosoftIdentityWebChallengeUserException msChallengeUserExceptionex)
            {
                await _tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(_productApiSettings.Scopes, msChallengeUserExceptionex.MsalUiRequiredException);
                throw;

            }

            return accessToken;
        }
    }
}
