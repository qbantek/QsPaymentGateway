using System.Collections.Generic;

namespace QsPaymentGateway
{
    public interface IGatewayApi
    {
        /// <summary>
        /// Gets or sets the API keys.
        /// </summary>
        /// <value>
        /// The API keys.
        /// </value>
        IList<string> ApiKeys { get; set; }

        /// <summary>
        /// Finds out if the API contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        bool ApiContainsKey(string key);
    }
}