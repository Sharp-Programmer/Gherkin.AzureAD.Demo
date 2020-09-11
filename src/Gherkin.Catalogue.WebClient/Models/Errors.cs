using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gherkin.Catalogue.WebClient.Models
{
    public class ErrorResult
    {
        public ErrorResult(string errorMessage) : this(errorMessage, null)
        {
            

        }

        public ErrorResult(string errorMessage, Exception exception)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public string ErrorMessage { get; }
        public Exception Exception{ get; set; }
    }
}