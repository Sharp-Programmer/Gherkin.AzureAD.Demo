using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Gherkin.Catalogue.WebClient.Models
{
    public class JsonErrorResult : ContentResult
    {
        public JsonErrorResult(int statusCode, object serializableObject)
        {
            Content = Newtonsoft.Json.JsonConvert.SerializeObject(serializableObject);
            ContentType = "application/json";
            StatusCode = statusCode;
        }
    }
}
