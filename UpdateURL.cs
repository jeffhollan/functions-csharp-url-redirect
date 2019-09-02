using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;

namespace Hollan.Function
{
    public static class UpdateURL
    {
        [FunctionName("UpdateURL")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("glympse/url.txt", FileAccess.Write)] Stream output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string pattern = @"(https://glympse.com.*?)\&";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var decodedBody = System.Net.WebUtility.UrlDecode(requestBody);

            log.LogInformation($"Got decoded body {decodedBody}");
            
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = r.Match(decodedBody);

            string glympseUrl = m.Groups[1].Value;
            log.LogInformation($"Found URL: {glympseUrl}");

            await output.WriteAsync(Encoding.UTF8.GetBytes(glympseUrl));

            return new OkResult();
        }
    }
}
