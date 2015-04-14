using System.Collections.Generic;

namespace QsPaymentGateway.PayPal
{
    public class PayPalRequest : GatewayRequest
    {
        private const string TestUrl = "test";
        private const string LiveUrl = "live";

        public PaymentGatewayType GatewayType { get; set; }
        public bool IsTestMode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public GatewayRequest Request { get; set; }

        public PayPalRequest(string username, string password) : this(username, password, false)
        {
        }

        public PayPalRequest(string username, string password, bool isTestMode)
        {
            TestMode = isTestMode;

            Post = new Dictionary<string, string>();
            Queue(PayPalApi.ApiLogin, username);
            Queue(PayPalApi.TransactionKey, password);
            //// default settings
            //Queue(DurangoApi.Method, "creditcard");
        }

        private bool _testMode;

        public bool TestMode
        {
            get { return _testMode; }
            set
            {
                _testMode = value;
                PostUrl = _testMode ? TestUrl : LiveUrl;
            }
        }
        public override string PostUrl { get; set; }

        public override RequestAction ApiAction
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <param name="address">The address.</param>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <returns></returns>
        public override GatewayRequest AddCustomer(string id, string first, string last, string address, string city, string state, string zip)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest AddShipping(string first, string last, string address, string state, string zip)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest AddMerchantValue(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest AddInvoice(string invoiceNumber)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest Sale(string cardNumber, string expirationMonthAndYear, string cvv, decimal amount)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest Settle(string transactionId, decimal amount)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest Void(string transactionId)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest Refund(string transactionId, decimal amount, string cardNumber)
        {
            throw new System.NotImplementedException();
        }

        public override GatewayRequest Authorize(string cardNumber, string expirationMonthAndYear, string cvv, decimal amount)
        {
            throw new System.NotImplementedException();
        }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}