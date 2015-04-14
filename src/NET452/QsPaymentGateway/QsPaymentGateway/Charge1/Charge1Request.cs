using System.Collections.Generic;
using System.Globalization;

namespace QsPaymentGateway.Charge1
{
    public class Charge1Request : GatewayRequest
    {
        private const string TestUrl = "https://secure.charge1.com/api/transact.php";
        private const string LiveUrl = "https://secure.charge1.com/api/transact.php";

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge1Request"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Charge1Request(string username, string password)
            : this(username, password, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Charge1Request"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isTestMode">if set to <c>true</c> [is test mode].</param>
        public Charge1Request(string username, string password, bool isTestMode)
        {
            TestMode = isTestMode;

            Post = new Dictionary<string, string>();
            Queue(Charge1Api.ApiLogin, username);
            Queue(Charge1Api.Password, password);
            // default settings
            Queue(Charge1Api.Method, "creditcard");
        }

        /// <summary>
        /// Sets the API action.
        /// </summary>
        /// <param name="action">The action.</param>
        private void SetApiAction(RequestAction action)
        {
            string apiValue;
            switch (action)
            {
                case RequestAction.Authorize:
                    apiValue = "auth";
                    break;
                case RequestAction.Settle:
                    apiValue = "capture";
                    break;
                case RequestAction.Sale:
                    apiValue = "sale";
                    break;
                case RequestAction.Refund:
                    apiValue = "refund";
                    break;
                case RequestAction.Void:
                    apiValue = "void";
                    break;
                default:
                    apiValue = "auth";
                    break;
            }
            Queue(Charge1Api.TransactionType, apiValue);
            ApiAction = action;
        }

        private bool _testMode;

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
            AssertValidation(Charge1Api.ApiLogin, Charge1Api.Password);

            //each call has its own requirements... check each
            switch (ApiAction)
            {
                case RequestAction.Sale:
                case RequestAction.Authorize:
                    AssertValidation(Charge1Api.CreditCardNumber, Charge1Api.CreditCardExpiration,
                                     Charge1Api.Amount);
                    break;
                case RequestAction.Settle:
                    AssertValidation(Charge1Api.TransactionId);
                    break;
                case RequestAction.Refund:
                    AssertValidation(Charge1Api.TransactionId, Charge1Api.Amount);
                    break;
                case RequestAction.Void:
                    AssertValidation(Charge1Api.TransactionId);
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
        public override GatewayRequest AddCustomer(string id, string first, string last, string address, string city,
                                                   string state,
                                                   string zip)
        {
            Queue(Charge1Api.FirstName, first);
            Queue(Charge1Api.LastName, last);
            Queue(Charge1Api.Address, address);
            Queue(Charge1Api.City, city);
            Queue(Charge1Api.State, state);
            Queue(Charge1Api.Zip, zip);
            Queue(Charge1Api.Country, "US");

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
            Queue(Charge1Api.ShipFirstName, first);
            Queue(Charge1Api.ShipLastName, last);
            Queue(Charge1Api.ShipAddress, address);
            Queue(Charge1Api.ShipState, state);
            Queue(Charge1Api.ShipZip, zip);
            Queue(Charge1Api.ShipCountry, "US");
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
            Queue(Charge1Api.InvoiceNumber, invoiceNumber);
            return this;
        }

        #endregion

        #region Requests

        /// <summary>
        /// Authorizes the specified card number.
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
            Queue(Charge1Api.CreditCardNumber, cardNumber);
            Queue(Charge1Api.CreditCardExpiration, expirationMonthAndYear);
            Queue(Charge1Api.CreditCardCode, cvv);
            Queue(Charge1Api.Amount, amount.ToString(CultureInfo.InvariantCulture));
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
            Queue(Charge1Api.CreditCardNumber, cardNumber);
            Queue(Charge1Api.CreditCardExpiration, expirationMonthAndYear);
            Queue(Charge1Api.CreditCardCode, cvv);
            Queue(Charge1Api.Amount, amount.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Settles the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public override GatewayRequest Settle(string transactionId, decimal amount)
        {
            SetApiAction(RequestAction.Settle);
            Queue(Charge1Api.TransactionId, transactionId);
            if (amount > 0)
                Queue(Charge1Api.Amount, amount.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        /// <summary>
        /// Voids the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public override GatewayRequest Void(string transactionId)
        {
            SetApiAction(RequestAction.Void);
            Queue(Charge1Api.TransactionId, transactionId);
            return this;
        }

        /// <summary>
        /// Refunds the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <returns></returns>
        public override GatewayRequest Refund(string transactionId, decimal amount, string cardNumber)
        {
            SetApiAction(RequestAction.Refund);
            Queue(Charge1Api.TransactionId, transactionId);
            Queue(Charge1Api.Amount, amount.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        #endregion
    }
}