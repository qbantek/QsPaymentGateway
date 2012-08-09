06/11/2011
For Durango:

var durango = new GatewayInfo
                    {
                        GatewayId = 1,  /* your own identifier, cannot be null */
                        PaymentGatewayType = PaymentGatewayType.Durango,
                        Username = "demo",
                        Password = "password",
                        IsTest = true
                    };
var gateway = new Gateway(durango);

gateway.Request.AddCustomer("0001", "John", "Smith", "1001 Main St.", "Miami", "FL", "33021");
gateway.Request.AddInvoice("10001");
gateway.Request.Authorize("4111111111111111", "052013", "111", 100.00m);

var response = gateway.Send();


**************
For Charge1:

var charge1 = new GatewayInfo
                    {
                        GatewayId = 2, /* your own identifier, cannot be null */
                        PaymentGatewayType = PaymentGatewayType.Durango,
                        Username = "demo",
                        Password = "password",
                        IsTest = true
                    };
var gateway = new Gateway(charge1);

gateway.Request.AddCustomer("0001", "John", "Smith", "1001 Main St.", "Miami", "FL", "33021");
gateway.Request.AddInvoice("10002");
gateway.Request.Authorize("4111111111111111", "052013", "111", 100.00m);

var response = gateway.Send();


**************

For Authorize.Net:

var authorize = new GatewayInfo
                    {
                        GatewayId = 3, /* your own identifier, cannot be null */
                        PaymentGatewayType = PaymentGatewayType.AuthorizeDotNet,
                        Username = "[YOUR-AUTH.NET-USERNAME]",
                        Password = "[YOUR-AUTH.NET-PASSWORD]",
                        IsTest = true
                    };

var gateway = new Gateway(authorize);

gateway.Request.AddCustomer("0001", "John", "Smith", "1001 Main St.", "Miami", "FL", "33021");
gateway.Request.AddInvoice("10003");
gateway.Request.Authorize("4111111111111111", "052013", "111", 100.00m);

var response = gateway.Send();

