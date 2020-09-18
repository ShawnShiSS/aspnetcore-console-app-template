using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTemplate.Models
{
    /// <summary>
    ///     Response from the application run
    /// </summary>
    public class ApplicationRunResponse
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
