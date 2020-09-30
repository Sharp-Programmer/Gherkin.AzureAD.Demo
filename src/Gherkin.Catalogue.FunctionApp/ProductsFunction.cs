using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Gherkin.Catalogue.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace Gherkin.Catalogue.FunctionApp
{
    public class ProductsFunction
    {
        private readonly IConfidentialClientApplication _app;
        private readonly ProductApiSettings _productApiSettings;
        private readonly HttpClient _productClient;


        public ProductsFunction(
            IConfidentialClientApplication app,
            IOptionsMonitor<ProductApiSettings> optionsMonitor,
            IHttpClientFactory httpClientFactory)
        {
            _app = app;
            _productApiSettings = optionsMonitor.CurrentValue;
            _productClient = httpClientFactory.CreateClient(Constants.ProductClientName);
        }
        [FunctionName("ProductsFunction")]
        public async Task Run([TimerTrigger("%FunctionTimer%")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var result = await _app.AcquireTokenForClient(new[] { $"{_productApiSettings.ScopeUri}/.default" }).ExecuteAsync();

            var token = result.AccessToken;

            _productClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Constants.Bearer, token);

            var productResult = await _productClient.GetAsync(_productApiSettings.Url);

            if (!productResult.IsSuccessStatusCode)
            {
                log.LogError($"Product Result failes with response {productResult.ReasonPhrase}");
                return;
            }

            var content = await productResult.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(content);

            foreach (var item in products)
            {
                log.LogInformation($"Product Name: {item.Name}");
                log.LogInformation($"Product Price: {item.Price}");
                log.LogInformation($"Product quantity: {item.AvailableQuantity}");
                log.LogInformation("---------------------------------------------");
            }
        }
    }
}
