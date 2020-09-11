using System;
using System.Collections.Generic;
using Microsoft.Graph;

namespace Gherkin.Catalogue.WebClient.Models
{
    public class ProductApiSettings
    {
        public Uri BaseAddress { get; set; }
        public string Url { get; set; }
        public string[] Scopes { get; set; }
    }
}