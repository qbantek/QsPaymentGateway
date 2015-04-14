namespace QsPaymentGateway
{
    public class GatewayInfo
    {
        /// <summary>
        /// Gets or sets the gateway id as in a local unique identifier for multi-gateways environments.
        /// </summary>
        /// <value>
        /// The gateway id.
        /// </value>
        public int GatewayId { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment gateway.
        /// </summary>
        /// <value>
        /// The type of the payment gateway.
        /// </value>
        public PaymentGatewayType PaymentGatewayType { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a test gateway.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this is a test gateway; otherwise, <c>false</c>.
        /// </value>
        public bool IsTest { get; set; }
    }
}