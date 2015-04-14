using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace QsPaymentGateway.AuthorizeDotNet
{
    public class AuthorizeDotNetResponse : IGatewayResponse
    {
        public string[] RawResponse { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeDotNetResponse"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="delimitCharacter">The delimit character.</param>
        public AuthorizeDotNetResponse(string result, char delimitCharacter)
        {
            var rawResponse = result.Split(delimitCharacter);
            if (rawResponse.Length == 1)
                throw new InvalidDataException(
                    string.Format(
                        "There was an error returned from AuthorizeNet: {0}; " +
                        "this usually means your data sent along was incorrect. " +
                        "Please recheck that all dates and amounts are formatted correctly",
                        rawResponse[0]));
            RawResponse = rawResponse;
        }

        public string MD5Hash
        {
            get { return ParseResponse(37); }
        }

        public string CcvCode
        {
            get { return ParseResponse(38); }
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
                        return "The Card Code should be on card, but is not indicated";
                    case "U":
                        return "Card Code is not supported by the card issuer";
                }
                return "";
            }
        }

        public int Code
        {
            get { return ParseInt(0); }
        }

        public int SubCode
        {
            get { return ParseInt(1); }
        }

        public string TransactionType
        {
            get { return ParseResponse(11); }
        }

        public string AuthorizationCode
        {
            get { return ParseResponse(4); }
        }

        public string Method
        {
            get { return ParseResponse(10); }
        }

        public decimal Amount
        {
            get { return ParseDecimal(9); }
        }

        public decimal Tax
        {
            get { return ParseDecimal(32); }
        }

        public string TransactionId
        {
            get { return ParseResponse(6); }
        }

        public string Message
        {
            get { return ParseResponse(3); }
        }

        public string FullResponse
        {
            get { return RawResponse.ToString(); }
        }

        public string InvoiceNumber
        {
            get { return ParseResponse(7); }
        }

        public string Description
        {
            get { return ParseResponse(8); }
        }

        public string ResponseCode
        {
            get { return ParseResponse(0); }
        }

        public string CardNumber
        {
            get { return ParseResponse(40); }
        }

        public string CardType
        {
            get { return ParseResponse(51); }
        }

        public string AvsCode
        {
            get { return ParseResponse(5); }
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
                        return "Address information not provided for AVS check";
                    case "E":
                        return "AVS error";
                    case "G":
                        return "Non-U.S. Card Issuing Bank";
                    case "N":
                        return "No Match on Address (Street) or ZIP";
                    case "P":
                        return "AVS not applicable for this transaction";
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

        public bool HeldForReview
        {
            get { return Code == 4; }
        }

        #endregion

        #region Address

        public string FirstName
        {
            get { return ParseResponse(13); }
        }

        public string LastName
        {
            get { return ParseResponse(14); }
        }

        public string Email
        {
            get { return ParseResponse(23); }
        }

        public string Company
        {
            get { return ParseResponse(15); }
        }

        public string Address
        {
            get { return ParseResponse(16); }
        }

        public string City
        {
            get { return ParseResponse(17); }
        }

        public string State
        {
            get { return ParseResponse(18); }
        }

        public string ZipCode
        {
            get { return ParseResponse(19); }
        }

        public string Country
        {
            get { return ParseResponse(20); }
        }

        #endregion

        #region Shipping

        public string ShipFirstName
        {
            get { return ParseResponse(24); }
        }

        public string ShipLastName
        {
            get { return ParseResponse(25); }
        }

        public string ShipCompany
        {
            get { return ParseResponse(26); }
        }

        public string ShipAddress
        {
            get { return ParseResponse(27); }
        }

        public string ShipCity
        {
            get { return ParseResponse(28); }
        }

        public string ShipState
        {
            get { return ParseResponse(29); }
        }

        public string ShipZipCode
        {
            get { return ParseResponse(30); }
        }

        public string ShipCountry
        {
            get { return ParseResponse(31); }
        }

        #endregion

        /// <summary>
        /// Parses the response to int.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private int ParseInt(int index)
        {
            var result = 0;
            if (RawResponse.Length > index)
                int.TryParse(RawResponse[index].ToString(CultureInfo.InvariantCulture), out result);
            return result;
        }

        /// <summary>
        /// Parses the response to decimal.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private decimal ParseDecimal(int index)
        {
            decimal result = 0;
            if (RawResponse.Length > index)
                decimal.TryParse(RawResponse[index].ToString(CultureInfo.InvariantCulture), out result);
            return result;
        }

        /// <summary>
        /// Parses the response.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private string ParseResponse(int index)
        {
            var result = string.Empty;
            if (RawResponse.Length > index)
                result = RawResponse[index].ToString(CultureInfo.InvariantCulture);
            return result;
        }

        /// <summary>
        /// All of the possible keys returned from the API
        /// </summary>
        public Dictionary<int, string> ApiReponseKeys
        {
            get
            {
                var result = new Dictionary<int, string>
                                 {
                                     {1, "Response Code"},
                                     {2, "Response Subcode"},
                                     {3, "Response Reason Code"},
                                     {4, "Response Reason Text"},
                                     {5, "Authorization Code"},
                                     {6, "AVS Response"},
                                     {7, "Transaction ID"},
                                     {8, "Invoice Number"},
                                     {9, "Description"},
                                     {10, "Amount"},
                                     {11, "Method"},
                                     {12, "Transaction Type"},
                                     {13, "Customer ID"},
                                     {14, "First Name"},
                                     {15, "Last Name"},
                                     {16, "Company"},
                                     {17, "Address"},
                                     {18, "City"},
                                     {19, "State"},
                                     {20, "ZIP Code"},
                                     {21, "Country"},
                                     {22, "Phone"},
                                     {23, "Fax"},
                                     {24, "Email Address"},
                                     {25, "Ship To First Name"},
                                     {26, "Ship To Last Name"},
                                     {27, "Ship To Company"},
                                     {28, "Ship To Address"},
                                     {29, "Ship To City"},
                                     {30, "Ship To State"},
                                     {31, "Ship To ZIP Code"},
                                     {32, "Ship To Country"},
                                     {33, "Tax"},
                                     {34, "Duty"},
                                     {35, "Freight"},
                                     {36, "Tax Exempt"},
                                     {37, "Purchase Order Number"},
                                     {38, "MD5 Hash"},
                                     {39, "Card Code Response"},
                                     {40, "Cardholder Authentication Verification Response"},
                                     {41, "Account Number"},
                                     {42, "Card Type"},
                                     {43, "Split Tender ID"},
                                     {44, "Requested Amount"},
                                     {45, "Balance On Card"}
                                 };
                return result;
            }
        }

        /// <summary>
        /// Finds the Response element by value.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public int FindByValue(string val)
        {
            var result = 0;
            for (var i = 0; i < RawResponse.Length; i++)
            {
                if (RawResponse[i].ToString(CultureInfo.InvariantCulture) != val)
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
            var index = 0;
            foreach (var key in ApiReponseKeys.Keys)
            {
                sb.AppendFormat("{0} = {1}\n", ApiReponseKeys[key], ParseResponse(index));
                index++;
            }
            return sb.ToString();
        }
    }
}