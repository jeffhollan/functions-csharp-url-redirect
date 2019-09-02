using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hollan.Function
{
    public static class Redirect
    {
        [FunctionName("Redirect")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("glympse/url.txt", FileAccess.Read)] string input,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var redirectionResponse = new RedirectResult(input, true);

            return redirectionResponse;
        }
    }
}
