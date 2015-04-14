using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace QsPaymentGateway.PayPal
{
    public class PayPalResponse : IGatewayResponse
    {
        public NameValueCollection ResponseValueCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalResponse"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public PayPalResponse(string result)
        {
            // Parse the query string variables into a NameValueCollection.
            var nameValueCollection = HttpUtility.ParseQueryString(result);
            if (nameValueCollection.Count == 0)
                throw new InvalidDataException(
                    "There was an error returned from PayPal. " +
                    "This usually means your data sent along was incorrect. " +
                    "Please recheck that all dates and amounts are formatted correctly");

            ResponseValueCollection = nameValueCollection;
        }

        public decimal Amount
        {
            get { throw new System.NotImplementedException(); }
        }

        public string TransactionId
        {
            get { throw new System.NotImplementedException(); }
        }

        public string AuthorizationCode
        {
            get { throw new System.NotImplementedException(); }
        }

        public string ResponseCode
        {
            get { throw new System.NotImplementedException(); }
        }

        public int Code
        {
            get { return ParseInt("response"); }
        }

        public string Message
        {
            get { throw new System.NotImplementedException(); }
        }

        public string FullResponse
        {
            get { throw new System.NotImplementedException(); }
        }

        public string AvsCode
        {
            get { throw new System.NotImplementedException(); }
        }

        public string AvsResponse
        {
            get { throw new System.NotImplementedException(); }
        }

        public string CcvCode
        {
            get { throw new System.NotImplementedException(); }
        }

        public string CcvResponse
        {
            get { throw new System.NotImplementedException(); }
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
    }
}