using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawboardProjects.Core.Config.Sections
{
    /// <summary>
    /// Configuration for Api
    /// </summary>
    public class Api
    {
        /// <summary>
        /// Base Address to be used in the API endpoint Uri.
        /// </summary>
        public string BaseAddress { get; set; }
        /// <summary>
        /// The version of API.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Subscription key which can be used in the Http client request header.
        /// </summary>
        public string SubscriptionKey { get; set; }
    }
}
