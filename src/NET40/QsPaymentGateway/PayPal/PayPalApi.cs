using System;

namespace QsPaymentGateway.PayPal
{
    public class PayPalApi : GatewayRequest
    {
        public static string ApiLogin;
        public static string TransactionKey;

        #region Overrides of GatewayRequest

        public override string PostUrl { get; set; }
        public override RequestAction ApiAction { get; set; }
        public override GatewayRequest AddCustomer(string id, string first, string last, string address, string city, string state, string zip)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest AddShipping(string first, string last, string address, string state, string zip)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest AddMerchantValue(string key, string value)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest AddInvoice(string invoiceNumber)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest Sale(string cardNumber, string expirationMonthAndYear, string cvv, decimal amount)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest Settle(string transactionId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest Void(string transactionId)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest Refund(string transactionId, decimal amount, string cardNumber)
        {
            throw new NotImplementedException();
        }

        public override GatewayRequest Authorize(string cardNumber, string expirationMonthAndYear, string cvv, decimal amount)
        {
            throw new NotImplementedException();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}