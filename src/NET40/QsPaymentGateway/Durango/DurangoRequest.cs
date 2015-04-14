using System.Collections.Generic;
using System.Globalization;

namespace QsPaymentGateway.Durango
{
    public class DurangoRequest : GatewayRequest
    {
        #region private members & constructors

        private const string TestUrl = "https://secure.charge1.com/api/transact.php";
        private const string LiveUrl = "https://secure.charge1.com/api/transact.php";
        private bool _testMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DurangoRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public DurangoRequest(string username, string password)
            : this(username, password, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurangoRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isTestMode">if set to <c>true</c> [is test mode].</param>
        public DurangoRequest(string username, string password, bool isTestMode)
        {
            TestMode = isTestMode;

            Post = new Dictionary<string, string>();
            Queue(DurangoApi.ApiLogin, username);
            Queue(DurangoApi.Password, password);
            // default settings
            Queue(DurangoApi.Method, "creditcard");
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
            Queue(DurangoApi.TransactionType, apiValue);
            ApiAction = action;
        }

        #endregion

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

        public override RequestAction ApiAction { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public override void Validate()
        {
            //make sure we have all the fields we need
            //starting with the login/key pair
            AssertValidation(DurangoApi.ApiLogin, DurangoApi.Password);

            //each call has its own requirements... check each
            switch (ApiAction)
            {
                case RequestAction.Sale:
                case RequestAction.Authorize:
                    AssertValidation(DurangoApi.CreditCardNumber, DurangoApi.CreditCardExpiration,
                                     DurangoApi.Amount);
                    break;
                case RequestAction.Settle:
                    AssertValidation(DurangoApi.TransactionId);
                    break;
                case RequestAction.Refund:
                    AssertValidation(DurangoApi.TransactionId, DurangoApi.Amount);
                    break;
                case RequestAction.Void:
                    AssertValidation(DurangoApi.TransactionId);
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
            Queue(DurangoApi.FirstName, first);
            Queue(DurangoApi.LastName, last);
            Queue(DurangoApi.Address, address);
            Queue(DurangoApi.State, state);
            Queue(DurangoApi.Zip, zip);
            Queue(DurangoApi.Country, "US");
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
            Queue(DurangoApi.ShipFirstName, first);
            Queue(DurangoApi.ShipLastName, last);
            Queue(DurangoApi.ShipAddress, address);
            Queue(DurangoApi.ShipState, state);
            Queue(DurangoApi.ShipZip, zip);
            Queue(DurangoApi.ShipCountry, "US");
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
            Queue(DurangoApi.InvoiceNumber, invoiceNumber);
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
            Queue(DurangoApi.CreditCardNumber, cardNumber);
            Queue(DurangoApi.CreditCardExpiration, expirationMonthAndYear);
            Queue(DurangoApi.CreditCardCode, cvv);
            Queue(DurangoApi.Amount, amount.ToString(CultureInfo.InvariantCulture));
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
            Queue(DurangoApi.CreditCardNumber, cardNumber);
            Queue(DurangoApi.CreditCardExpiration, expirationMonthAndYear);
            Queue(DurangoApi.CreditCardCode, cvv);
            Queue(DurangoApi.Amount, amount.ToString(CultureInfo.InvariantCulture));
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
            Queue(DurangoApi.TransactionId, transactionId);
            if (amount > 0)
                Queue(DurangoApi.Amount, amount.ToString(CultureInfo.InvariantCulture));
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
            Queue(DurangoApi.TransactionId, transactionId);
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
            Queue(DurangoApi.TransactionId, transactionId);
            Queue(DurangoApi.Amount, amount.ToString(CultureInfo.InvariantCulture));
            return this;
        }

        #endregion
    }
}