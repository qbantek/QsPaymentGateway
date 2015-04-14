using System.Collections.Generic;
using System.IO;
using System.Net;
using QsPaymentGateway.AuthorizeDotNet;
using QsPaymentGateway.Charge1;
using QsPaymentGateway.Durango;
using QsPaymentGateway.PayPal;

namespace QsPaymentGateway
{
    public class Gateway : IGateway
    {
        #region Private members and constructors

        public PaymentGatewayType GatewayType { get; set; }
        public bool IsTestMode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public GatewayRequest Request { get; set; }
        public Dictionary<string, string> Post { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class.
        /// </summary>
        /// <param name="gatewayType">Type of the gateway.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Gateway(PaymentGatewayType gatewayType, string username, string password)
            : this(gatewayType, username, password, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class.
        /// </summary>
        /// <param name="gatewayInfo">The gateway info.</param>
        public Gateway(GatewayInfo gatewayInfo)
            : this(gatewayInfo.PaymentGatewayType, gatewayInfo.Username, gatewayInfo.Password, gatewayInfo.IsTest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class.
        /// </summary>
        /// <param name="gatewayType">Type of the gateway.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isTestMode">if set to <c>true</c> [is test mode].</param>
        public Gateway(PaymentGatewayType gatewayType, string username, string password, bool isTestMode)
        {
            GatewayType = gatewayType;
            Username = username;
            Password = password;
            IsTestMode = isTestMode;

            switch (gatewayType)
            {
                case PaymentGatewayType.AuthorizeDotNet:
                    Request = new AuthorizeDotNetRequest(username, password, isTestMode);
                    break;
                case PaymentGatewayType.Durango:
                    Request = new DurangoRequest(username, password, isTestMode);
                    break;
                case PaymentGatewayType.Paypal:
                    Request = new PayPalRequest(username, password, isTestMode);
                    break;
                case PaymentGatewayType.Charge1:
                    Request = new Charge1Request(username, password, isTestMode);
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Sends this instance.
        /// </summary>
        /// <returns></returns>
        public IGatewayResponse Send()
        {
            return Send(Request);
        }

        /// <summary>
        /// Sends the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public IGatewayResponse Send(GatewayRequest request)
        {
            //validate the inputs
            request.Validate();
            var postData = request.ToPostString();

            //override the local cert policy
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            var serviceUrl = request.PostUrl;
            var webRequest = (HttpWebRequest) WebRequest.Create(serviceUrl);
            webRequest.Method = "POST";
            webRequest.ContentLength = postData.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            // post data is sent as a stream
            var myWriter = new StreamWriter(webRequest.GetRequestStream());
            myWriter.Write(postData);
            myWriter.Close();

            // returned values are returned as a stream, then read into a string
            var response = (HttpWebResponse) webRequest.GetResponse();
            var rawResponseStream = response.GetResponseStream();

            var result = string.Empty;
            if (rawResponseStream != null)
                using (var responseStream = new StreamReader(rawResponseStream))
                {
                    result = responseStream.ReadToEnd();
                    responseStream.Close();
                }

            IGatewayResponse gatewayResponse = null;
            switch (GatewayType)
            {
                case PaymentGatewayType.AuthorizeDotNet:
                    gatewayResponse = new AuthorizeDotNetResponse(result,
                                                                  request.Post[AuthorizeDotNetApi.DelimitCharacter].
                                                                      ToCharArray()
                                                                      [0]);
                    break;
                case PaymentGatewayType.Durango:
                    gatewayResponse = new DurangoResponse(result);
                    break;
                case PaymentGatewayType.Paypal:
                    gatewayResponse = new PayPalResponse(result);
                    break;
                case PaymentGatewayType.Charge1:
                    gatewayResponse = new Charge1Response(result);
                    break;
            }

            return gatewayResponse;
        }
    }
}