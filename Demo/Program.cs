using System;
using System.Globalization;
using QsPaymentGateway;

namespace Demo
{
    public class Program
    {
        private static void Main()
        {
            try
            {
                var durango = new GatewayInfo
                                  {
                                      GatewayId = 1,
                                      PaymentGatewayType = PaymentGatewayType.Durango,
                                      Username = "demo",
                                      Password = "password",
                                      IsTest = true
                                  };
                IdentifyGatewayToConsole(durango);
                var response = SendAuthRequest(durango);
                ResponseToConsole(response);

                var charge1 = new GatewayInfo
                                  {
                                      GatewayId = 2,
                                      PaymentGatewayType = PaymentGatewayType.Charge1,
                                      Username = "demo",
                                      Password = "password",
                                      IsTest = true
                                  };
                IdentifyGatewayToConsole(charge1);
                response = SendAuthRequest(charge1);
                ResponseToConsole(response);

                var authorize = new GatewayInfo
                                    {
                                        GatewayId = 3,
                                        PaymentGatewayType = PaymentGatewayType.AuthorizeDotNet,
                                        Username = "auth.net-user",
                                        Password = "auth.net-password",
                                        IsTest = true
                                    };
                IdentifyGatewayToConsole(authorize);
                response = SendAuthRequest(authorize);
                ResponseToConsole(response);

                Console.WriteLine("");
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Responses to console.
        /// </summary>
        /// <param name="response">The response.</param>
        private static void ResponseToConsole(IGatewayResponse response)
        {
            Console.WriteLine(response.TransactionId);
            Console.WriteLine(response.Message);
            Console.WriteLine(response.AvsResponse);
            Console.WriteLine(response.CcvResponse);
        }

        /// <summary>
        /// Sends the auth request.
        /// </summary>
        /// <param name="gatewayInfo">The gateway info.</param>
        /// <returns></returns>
        private static IGatewayResponse SendAuthRequest(GatewayInfo gatewayInfo)
        {

            var gateway = new Gateway(gatewayInfo);
            gateway.Request.AddCustomer("00001", "John", "Smith", "1000 Main St.", "Miami", "FL", "33021");
            var random = new Random();
            var randomNumber = random.Next(0, 10000);
            gateway.Request.AddInvoice(randomNumber.ToString(CultureInfo.InvariantCulture));

            var expDate = DateTime.Today.AddYears(1).ToString("MMyyyy");
            gateway.Request.Authorize("4111111111111111", expDate, "111", 100.00m);
            var response = gateway.Send();
            return response;
        }

        /// <summary>
        /// Identifies the gateway to console.
        /// </summary>
        /// <param name="gatewayInfo">The gateway info.</param>
        private static void IdentifyGatewayToConsole(GatewayInfo gatewayInfo)
        {
            Console.WriteLine("***********************************");
            Console.WriteLine(gatewayInfo.PaymentGatewayType.ToString().ToUpper());
            Console.WriteLine("***********************************");
        }
    }
}