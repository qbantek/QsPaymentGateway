namespace QsPaymentGateway
{
    public interface IGateway
    {
        bool IsTestMode { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        GatewayRequest Request { get; set; }

        IGatewayResponse Send(GatewayRequest request);
    }
}
