using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;

namespace QsPaymentGateway.Durango
{
    public class DurangoResponse : IGatewayResponse
    {
        public NameValueCollection ResponseValueCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DurangoResponse"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public DurangoResponse(string result)
        {
            // Parse the query string variables into a NameValueCollection.
            var nameValueCollection = HttpUtility.ParseQueryString(result);
            if (nameValueCollection.Count == 0)
                throw new InvalidDataException(
                    "There was an error returned from Charge 1. " +
                    "This usually means your data sent along was incorrect. " +
                    "Please recheck that all dates and amounts are formatted correctly");

            ResponseValueCollection = nameValueCollection;
        }

        public int Code
        {
            get { return ParseInt("response"); }
        }

        public string CcvCode
        {
            get { return ParseResponse("cvvresponse"); }
        }

        public string CcvResponse
        {
            get
            {
                var code = CcvCode;

                switch (code)
                {
                    case "M":
                        return "Successful Match";
                    case "N":
                        return "The Card Code does not match";
                    case "P":
                        return "The Card Code was not processed";
                    case "S":
                        return "Merchant has indicated that the Card Code is not present on card";
                    case "U":
                        return "Card Code is not supported by the card issuer";
                }
                return "";
            }
        }

        public string AvsCode
        {
            get { return ParseResponse("avsresponse"); }
        }

        public string AvsResponse
        {
            get
            {
                var code = AvsCode;

                switch (code)
                {
                    case "A":
                        return "Address (Street) matches, ZIP does not";
                    case "B":
                        return "Address (Street) matches, ZIP does not";
                    case "C":
                        return "No Match on Address (Street) or ZIP";
                    case "D":
                        return "Five digit ZIP matches, Address (Street) does not";
                    case "E":
                        return "Not a mail/phone order";
                    case "G":
                        return "Non-U.S. Card Issuing Bank";
                    case "I":
                        return "Non-U.S. Card Issuing Bank";
                    case "L":
                        return "Five digit ZIP matches, Address (Street) does not";
                    case "N":
                        return "No Match on Address (Street) or ZIP";
                    case "M":
                        return "Five digit ZIP matches, Address (Street) does not";
                    case "O":
                        return "AVS not available";
                    case "P":
                        return "Five digit ZIP matches, Address (Street) does not";
                    case "R":
                        return "Retry — System unavailable or timed out";
                    case "S":
                        return "Service not supported by issuer";
                    case "U":
                        return "Address information is unavailable";
                    case "W":
                        return "Nine digit ZIP matches, Address (Street) does not";
                    case "X":
                        return "Address (Street) and nine digit ZIP match";
                    case "Y":
                        return "Address (Street) and five digit ZIP match";
                    case "Z":
                        return "Five digit ZIP matches, Address (Street) does not";
                    case "0":
                        return "AVS not available";
                }
                return "";
            }
        }

        public string AuthorizationCode
        {
            get { return ParseResponse("authcode"); }
        }

        public decimal Amount
        {
            get { return 0; }
        }

        public string TransactionId
        {
            get { return ParseResponse("transactionid"); }
        }

        public string Message
        {
            get { return Response; }
        }

        public string FullResponse
        {
            get { return ResponseValueCollection.ToString(); }
        }

        public string InvoiceNumber
        {
            get { return ParseResponse("orderid"); }
        }

        public string ResponseCode
        {
            get { return ParseResponse("response_code"); }
        }

        public string Response
        {
            get
            {
                var code = ResponseCode;

                switch (code)
                {
                    case "100":
                        return "Transaction was Approved.";

                    case "200":
                        return "Transaction was Declined by Processor.";
                    case "201":
                        return "Do Not Honor.";
                    case "202":
                        return "Insufficient Funds.";
                    case "203":
                        return "Over Limit.";
                    case "204":
                        return "Transaction not allowed.";
                    case "220":
                        return "Incorrect Payment Data.";
                    case "221":
                        return "No such card issuer.";
                    case "222":
                        return "No card number on file with issuer.";
                    case "224":
                        return "Invalid expiration date.";
                    case "225":
                        return "Invalid security card code.";
                    case "240":
                        return "Call Issuer for Further Information.";
                    case "250":
                        return "Pick Up Card.";
                    case "251":
                        return "Lost card.";
                    case "252":
                        return "Stolen card.";
                    case "253":
                        return "Fraudulent card.";
                    case "260":
                        return "Declined with further Instructions Available (see response text)";
                    case "261":
                        return "Declined. Stop ALL recurring payments.";
                    case "262":
                        return "Declined. Stop this recurring program.";
                    case "263":
                        return "Declined. Update cardholder data available.";
                    case "264":
                        return "Declined. Retry in a few days.";

                    case "300":
                        return "Transaction was Rejected by Gateway.";

                    case "400":
                        return "Transaction Error Returned by Processor.";
                    case "410":
                        return "Invalid Merchant Configuration.";
                    case "420":
                        return "Communication Error.";
                    case "421":
                        return "Communication Error with Issuer.";
                    case "430":
                        return "Duplicate Transaction at Processor.";
                    case "440":
                        return "Processor Format Error.";
                    case "441":
                        return "Invalid transaction information.";
                    case "460":
                        return "Processor feature not available.";
                    case "461":
                        return "Unsupported card type.";
                }
                return "";
            }
        }

        #region Status

        public bool Approved
        {
            get { return Code == 1; }
        }

        public bool Declined
        {
            get { return Code == 2; }
        }

        public bool Error
        {
            get { return Code == 3; }
        }

        #endregion

        /// <summary>
        /// Parses the int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private int ParseInt(string key)
        {
            var result = 0;
            if (ResponseValueCollection.Count > 0)
                int.TryParse(ResponseValueCollection[key], out result);
            return result;
        }

        /// <summary>
        /// Parses the response.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string ParseResponse(string key)
        {
            var result = string.Empty;
            if (ResponseValueCollection.Count > 0)
                result = ResponseValueCollection[key];
            return result;
        }

        /// <summary>
        /// All of the possible keys returned from the API
        /// </summary>
        public Dictionary<string, string> ApiResponseKeys
        {
            get
            {
                var result = new Dictionary<string, string>
                                 {
                                     {"response", "Response"},
                                     {"responsetext", "Response Reason Text"},
                                     {"authcode", "Authorization Code"},
                                     {"transactionid", "Transaction ID"},
                                     {"avsresponse", "AVS Response"},
                                     {"cvvresponse", "Cardholder Authentication Verification Response"},
                                     {"orderid", "Invoice Number"},
                                     {"response_code", "Response Code"}
                                 };
                return result;
            }
        }

        /// <summary>
        /// Finds the by value.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public int FindByValue(string val)
        {
            var result = 0;
            for (var i = 0; i < ResponseValueCollection.Count; i++)
            {
                if (ResponseValueCollection[i] != val)
                    continue;
                result = i;
                break;
            }
            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var key in ApiResponseKeys.Keys)
                sb.AppendFormat("{0} = {1}\n", ApiResponseKeys[key], ParseResponse(key));

            return sb.ToString();
        }
    }
}