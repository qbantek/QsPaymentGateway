using System.Collections.Generic;

namespace QsPaymentGateway.AuthorizeDotNet
{
    public class AuthorizeDotNetRequest : GatewayRequest
    {
        #region private members & constructors

        private const string TestUrl = "https://test.authorize.net/gateway/transact.dll";
        private const string LiveUrl = "https://secure.authorize.net/gateway/transact.dll";
        private bool _testMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeDotNetRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public AuthorizeDotNetRequest(string username, string password)
            : this(username, password, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeDotNetRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isTestMode">if set to <c>true</c> [is test mode].</param>
        public AuthorizeDotNetRequest(string username, string password, bool isTestMode)
        {
            TestMode = isTestMode;

            Post = new Dictionary<string, string>();
            Queue(AuthorizeDotNetApi.ApiLogin, username);
            Queue(AuthorizeDotNetApi.TransactionKey, password);
            // default settings
            Queue(AuthorizeDotNetApi.DelimitData, "TRUE");
            Queue(AuthorizeDotNetApi.DelimitCharacter, "|");
            Queue(AuthorizeDotNetApi.RelayResponse, "TRUE");
            Queue(AuthorizeDotNetApi.EmailCustomer, "FALSE");
            Queue(AuthorizeDotNetApi.Method, "CC");
            Queue(AuthorizeDotNetApi.Country, "US");
            Queue(AuthorizeDotNetApi.ShipCountry, "US");
            Queue(AuthorizeDotNetApi.DuplicateWindowTime, "120");
        }

        /// <summary>
        /// Sets the API action.
        /// </summary>
        /// <param name="action">The action.</param>
        private void SetApiAction(RequestAction action)
        {
            var apiValue = "AUTH_CAPTURE";

            ApiAction = action;
            switch (action)
            {
                case RequestAction.Authorize:
                    apiValue = "AUTH_ONLY";
                    break;
                case RequestAction.Settle:
                    apiValue = "PRIOR_AUTH_CAPTURE";
                    break;
                case RequestAction.Refund:
                    apiValue = "CREDIT";
                    break;
                case RequestAction.Void:
                    apiValue = "VOID";
                    break;
            }
            Queue(AuthorizeDotNetApi.TransactionType, apiValue);
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether [test mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [test mode]; otherwise, <c>false</c>.
        /// </value>
        public bool TestMode
        {
            get { return _testMode; }
            set
            {
                _testMode = value;
                PostUrl = _testMode ? TestUrl : LiveUrl;
            }
        }

        /// <summary>
        /// Gets or sets the post URL.
        /// </summary>
        /// <value>
        /// The post URL.
        /// </value>
        public override string PostUrl { get; set; }

        /// <summary>
        /// Gets or sets the API action.
        /// </summary>
        /// <value>
        /// The API action.
        /// </value>
        public override RequestAction ApiAction { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public override void Validate()
        {
            //make sure we have all the fields we need
            //starting with the login/key pair
            AssertValidation(AuthorizeDotNetApi.ApiLogin, AuthorizeDotNetApi.TransactionKey);

            //each call has its own requirements... check each
            switch (ApiAction)
            {
                case RequestAction.Sale:
                case RequestAction.Authorize:
                    AssertValidation(AuthorizeDotNetApi.CreditCardNumber, AuthorizeDotNetApi.CreditCardExpiration,
                                     AuthorizeDotNetApi.Amount);
                    break;
                case RequestAction.Settle:
                    AssertValidation(AuthorizeDotNetApi.TransactionId);//, AuthorizeDotNetApi.AuthorizationCode);
                    break;
                case RequestAction.Refund:
                    AssertValidation(AuthorizeDotNetApi.TransactionId, AuthorizeDotNetApi.Amount,
                                     AuthorizeDotNetApi.CreditCardNumber);
                    break;
                case RequestAction.Void:
                    AssertValidation(AuthorizeDotNetApi.TransactionId);
                    break;
            }
        }

        #region Fluent stuff

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
        public override GatewayRequest AddCustomer(string id, string first, string last, string address, string city, string state,
                                                   string zip)
        {
            Queue(AuthorizeDotNetApi.FirstName, first);
            Queue(AuthorizeDotNetApi.LastName, last);
            Queue(AuthorizeDotNetApi.Address, address);
            Queue(AuthorizeDotNetApi.City, city);
            Queue(AuthorizeDotNetApi.State, state);
            Queue(AuthorizeDotNetApi.Zip, zip);
            Queue(AuthorizeDotNetApi.CustomerId, id);
            return this;
        }

        /// <summary>
        /// Adds the shipping.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="last">The last.</param>
        /// <param name="address">The address.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <returns></returns>
        public override GatewayRequest AddShipping(string first, string last, string address, string state, string zip)
        {
            Queue(AuthorizeDotNetApi.ShipFirstName, first);
            Queue(AuthorizeDotNetApi.ShipLastName, last);
            Queue(AuthorizeDotNetApi.ShipAddress, address);
            Queue(AuthorizeDotNetApi.ShipState, state);
            Queue(AuthorizeDotNetApi.ShipZip, zip);
            return this;
        }

        /// <summary>
        /// Adds the merchant value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override GatewayRequest AddMerchantValue(string key, string value)
        {
            Queue(key, value);
            return this;
        }

        /// <summary>
        /// Adds the invoice.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number.</param>
        /// <returns></returns>
        public override GatewayRequest AddInvoice(string invoiceNumber)
        {
            Queue(AuthorizeDotNetApi.InvoiceNumber, invoiceNumber);
            return this;
        }

        #endregion

        #region Requests

        /// <summary>
        /// Authorizes a charge for the specified amount on the given credit card.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="expirationMonthAndYear">The expiration month and year.</param>
        /// <param name="cvv">The CVV.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override GatewayRequest Authorize(string cardNumber, string expirationMonthAndYear, string cvv,
                                                 decimal amount)
        {
            SetApiAction(RequestAction.Authorize);
            Queue(AuthorizeDotNetApi.CreditCardNumber, cardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, expirationMonthAndYear);
            Queue(AuthorizeDotNetApi.CreditCardCode, cvv);
            Queue(AuthorizeDotNetApi.Amount, amount.ToString());
            return this;
        }

        /// <summary>
        /// Sales the specified card number.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="expirationMonthAndYear">The expiration month and year.</param>
        /// <param name="cvv">The CVV.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override GatewayRequest Sale(string cardNumber, string expirationMonthAndYear, string cvv, decimal amount)
        {
            SetApiAction(RequestAction.Sale);
            Queue(AuthorizeDotNetApi.CreditCardNumber, cardNumber);
            Queue(AuthorizeDotNetApi.CreditCardExpiration, expirationMonthAndYear);
            Queue(AuthorizeDotNetApi.CreditCardCode, cvv);
            Queue(AuthorizeDotNetApi.Amount, amount.ToString());
            return this;
        }

        /// <summary>
        /// Settles the transaction that matches the specified transaction id and amount.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override GatewayRequest Settle(string transactionId, decimal amount)
        {
            SetApiAction(RequestAction.Settle);
            Queue(AuthorizeDotNetApi.TransactionId, transactionId);
            if (amount > 0) Queue(AuthorizeDotNetApi.Amount, amount.ToString());
            return this;
        }

        /// <summary>
        /// Voids the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public override GatewayRequest Void(string transactionId)
        {
            SetApiAction(RequestAction.Void);
            Queue(AuthorizeDotNetApi.TransactionId, transactionId);
            return this;
        }

        /// <summary>
        /// Refunds the transaction that matches the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <returns></returns>
        public override GatewayRequest Refund(string transactionId, decimal amount, string cardNumber)
        {
            SetApiAction(RequestAction.Refund);
            Queue(AuthorizeDotNetApi.TransactionId, transactionId);
            Queue(AuthorizeDotNetApi.CreditCardNumber, cardNumber);
            Queue(AuthorizeDotNetApi.Amount, amount.ToString());
            return this;
        }

        #endregion
    }
}