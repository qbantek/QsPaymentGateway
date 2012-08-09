using System.Collections.Generic;

namespace QsPaymentGateway.Durango
{
    public class DurangoApi : IGatewayApi
    {
        #region Api field names (constants)

        /// <summary>
        /// The merchant's unique API Username
        /// </summary>
        public const string ApiLogin = "username";

        /// <summary>
        /// The merchant's unique password
        /// </summary>	
        public const string Password = "password";

        /// <summary>
        /// The type of transaction:
        /// AUTH_CAPTURE (default), AUTH_ONLY, CAPTURE_ONLY, CREDIT, PRIOR_AUTH_CAPTURE, VOID
        /// </summary>
        public const string TransactionType = "type";

        /// <summary>
        /// CC or ECHECK
        /// </summary>
        public const string Method = "payment";

        /// <summary>
        /// The amount of the transaction
        /// </summary>
        public const string Amount = "amount";

        /// <summary>
        /// The credit card number - between 13 and 16 digits without spaces. When x_type=CREDIT, only the last four digits are required
        /// </summary>
        public const string CreditCardNumber = "ccnumber";

        /// <summary>
        /// The expiration date - MMYY, MM/YY, MM-YY, MMYYYY, MM/YYYY, MM-YYYY
        /// </summary>
        public const string CreditCardExpiration = "ccexp";

        /// <summary>
        /// The three- or four-digit number on the back of a credit card (on the front for American Express).
        /// </summary>
        public const string CreditCardCode = "cvv";

        /// <summary>
        /// The payment gateway assigned transaction ID of an original transaction - Required only for CREDIT, PRIOR_ AUTH_ CAPTURE, and VOID transactions
        /// </summary>
        public const string TransactionId = "transactionid";

        /// <summary>
        /// The authorization code of an original transaction not authorized on the payment gateway
        /// </summary>
        public const string AuthorizationCode = "authcode";

        /// <summary>
        /// The merchant assigned invoice number for the transaction
        /// </summary>
        public const string InvoiceNumber = "orderid";

        /// <summary>
        /// The transaction description
        /// </summary>
        public const string Description = "orderdescription";

        ///<summary>
        ///</summary>
        public const string FirstName = "firstname";

        ///<summary>
        ///</summary>
        public const string LastName = "lastname";

        ///<summary>
        ///</summary>
        public const string Company = "company";

        ///<summary>
        ///</summary>
        public const string Address = "address1";

        ///<summary>
        ///</summary>
        public const string City = "city";

        ///<summary>
        ///</summary>
        public const string State = "state";

        ///<summary>
        ///</summary>
        public const string Zip = "zip";

        ///<summary>
        ///</summary>
        public const string Country = "country";

        ///<summary>
        ///</summary>
        public const string Phone = "phone";

        ///<summary>
        ///</summary>
        public const string Fax = "fax";

        ///<summary>
        ///</summary>
        public const string Email = "email";

        ///<summary>
        ///</summary>
        public const string CustomerIpAddress = "ipaddress";

        ///<summary>
        ///</summary>
        public const string ShipFirstName = "shipping_firstname";

        ///<summary>
        ///</summary>
        public const string ShipLastName = "shipping_lastname";

        ///<summary>
        ///</summary>
        public const string ShipCompany = "shipping_company";

        ///<summary>
        ///</summary>
        public const string ShipAddress = "shipping_address1";

        ///<summary>
        ///</summary>
        public const string ShipCity = "shipping_city";

        ///<summary>
        ///</summary>
        public const string ShipState = "shipping_state";

        ///<summary>
        ///</summary>
        public const string ShipZip = "shipping_zip";

        ///<summary>
        ///</summary>
        public const string ShipCountry = "shipping_country";

        ///<summary>
        ///</summary>
        public const string Tax = "tax";

        ///<summary>
        ///</summary>
        public const string Freight = "shipping";

        ///<summary>
        ///</summary>
        public const string PoNumber = "ponumber";

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DurangoApi"/> class.
        /// </summary>
        public DurangoApi()
        {
            ApiKeys = new List<string>
                          {
                              "username",
                              "password",
                              "type",
                              "payment",
                              "amount",
                              "ccnumber",
                              "ccexp",
                              "cvv",
                              "transactionid",
                              "authcode",
                              "orderid",
                              "orderdescription",
                              "firstname",
                              "lastname",
                              "company",
                              "address1",
                              "city",
                              "state",
                              "zip",
                              "country",
                              "phone",
                              "fax",
                              "email",
                              "ipaddress",
                              "shipping_firstname",
                              "shipping_lastname",
                              "shipping_company",
                              "shipping_address1",
                              "shipping_city",
                              "shipping_state",
                              "shipping_zip",
                              "shipping_country",
                              "tax",
                              "shipping",
                              "ponumber"
                          };
        }

        /// <summary>
        /// Gets or sets the API keys.
        /// </summary>
        /// <value>
        /// The API keys.
        /// </value>
        public IList<string> ApiKeys { get; set; }

        /// <summary>
        /// Finds out if the API contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool ApiContainsKey(string key)
        {
            return ApiKeys.Contains(key);
        }
    }
}