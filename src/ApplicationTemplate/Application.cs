using ApplicationTemplate.Config;
using ApplicationTemplate.Models;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ApplicationTemplate
{
    public class Application
    {
        /// <summary>
        ///     Email settings from Configuration
        /// </summary>
        private readonly IOptions<EmailSettings> _emailSettings;

        /// <summary>
        ///     Constructor is used to inject services/IOptions that we need here
        /// </summary>
        /// <param name="emailSettings"></param>
        public Application(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
        }

        // Async application starting point
        public async Task<ApplicationRunResponse> Run()
        {
            ApplicationRunResponse response = new ApplicationRunResponse();

            // TODO : Write your own application code here.

            Log.Information($"{_emailSettings.Value.DefaultFromName} <{_emailSettings.Value.DefaultFromEmail}>");

            return response;
        }
    }
}
