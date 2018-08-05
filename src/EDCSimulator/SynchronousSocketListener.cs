using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EDCSimulator
{
    public class SynchronousSocketListener
    {
        // Incoming data from the client.  
        public string data = null;

        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    //while (true)
                    //{
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        //if (data.IndexOf("<EOF>") > -1)
                        //{
                        //    break;
                        //}
                    //}

                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", data);

                    // Send Ack
                    StringBuilder ackMessage = new StringBuilder();
                    ackMessage.Append(@"<?xml version=""1.0"" encoding=""UTF - 8""?>");
                    ackMessage.Append(@"<EFTAcknowledgement>");
                    ackMessage.Append(@"<AcknowledgementType>0003</AcknowledgementType>");
                    ackMessage.Append(@"</EFTAcknowledgement>");

                    ackMessage.Insert(0, "  ");
                    ackMessage.Insert(0, ackMessage.Length.ToString().PadLeft(5, '0'));
                    ackMessage.Insert(0, "RK");
                    byte[] ack = Encoding.ASCII.GetBytes(ackMessage.ToString());
                    handler.Send(ack);
                    Console.WriteLine("Sent acknowledgement: " + ackMessage.ToString());

                    Thread.Sleep(15000);


                    // Send Response
                    StringBuilder resMessage = new StringBuilder();
                    resMessage.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                    resMessage.Append(@"<EFTResponse>");
                    resMessage.Append(@"<ResponseType>0003</ResponseType>");
                    resMessage.Append(@"<MerchantRef>9400135000256</MerchantRef>");
                    resMessage.Append(@"<Status>1</Status>");
                    resMessage.Append(@"<ResponseCode>00</ResponseCode>");
                    resMessage.Append(@"<ResponseText>4500123</ResponseText>");
                    resMessage.Append(@"<AuthCode>456743</AuthCode>");
                    resMessage.Append(@"<InvoiceNumber>4500123</InvoiceNumber>");
                    resMessage.Append(@"<TerminalID>42711001</TerminalID>");
                    resMessage.Append(@"<MerchantID>64002711</MerchantID>");
                    resMessage.Append(@"<CardNumber>413599******1733</CardNumber>");
                    resMessage.Append(@"<ExpiryDate>0519</ExpiryDate>");
                    resMessage.Append(@"<TransactionDate>160625</TransactionDate>");
                    resMessage.Append(@"<TransactionTime>180623</TransactionTime>");
                    resMessage.Append(@"<RRN>180623</RRN>");
                    resMessage.Append(@"<TransactionType>Sale</TransactionType>");
                    resMessage.Append(@"<BaseCurrency>HKD</BaseCurrency>");
                    resMessage.Append(@"<BaseAmount>125.95</BaseAmount>");
                    resMessage.Append(@"<DCCCurrency>USD</DCCCurrency>");
                    resMessage.Append(@"<DCCAmount>16.74</DCCAmount>");
                    resMessage.Append(@"<DCCAmountMinorUnit>2</DCCAmountMinorUnit>");
                    resMessage.Append(@"<ExchangeRate>0.12975866</ExchangeRate>");
                    resMessage.Append(@"<ExchangeRateMinorUnit>8</ExchangeRateMinorUnit>");
                    resMessage.Append(@"<CardType>VISA</CardType>");
                    resMessage.Append(@"<ReceiptFlag>2</ReceiptFlag>");
                    resMessage.Append(@"<CashierID>1</CashierID>");
                    resMessage.Append(@"<BatchNumber>000416</BatchNumber>");
                    resMessage.Append(@"<EntryMethod>EMV</EntryMethod>");
                    resMessage.Append(@"<AuthMethod>0</AuthMethod>");
                    resMessage.Append(@"<CustomerReceipt>");
                    resMessage.Append(@"DATE: 2016-06-25 TIME: 18:06");
                    resMessage.Append(@"MID: 64002711 TID: 42711001");
                    resMessage.Append(@"BATCH: 000416 INVOICE: 4500123");
                    resMessage.Append(@"ECR REF: 9400135000256");
                    resMessage.Append(@"SALE");
                    resMessage.Append(@"CARD NUMBER: 413599******1733");
                    resMessage.Append(@"CARDHOLDER NAME:");
                    resMessage.Append(@"VISA EXPIRY: 0519");
                    resMessage.Append(@"RRN: 180623");
                    resMessage.Append(@"APPROVAL/AUTH CODE:");
                    resMessage.Append(@"APP: VISA DCC CREDIT");
                    resMessage.Append(@"AID: A0000000031010");
                    resMessage.Append(@"TC: B0FE935BE11C80CA");
                    resMessage.Append(@"TOTAL: HKD 125.95");
                    resMessage.Append(@"** DCC TRANSACTION **");
                    resMessage.Append(@"TRANSACTION CURRENCY: USD");
                    resMessage.Append(@"* EXCHANGE RATE: HKD = 0.12975866 USD");
                    resMessage.Append(@"Sale Amt: USD 16.74");
                    resMessage.Append(@"Customer signed electronically");
                    resMessage.Append(@"Cardholder acknowledges that the currency");
                    resMessage.Append(@"conversion is conducted by the Merchant");
                    resMessage.Append(@"and is not associated with or endorsed by");
                    resMessage.Append(@"Visa/Mastercard.");
                    resMessage.Append(@"Cardholder has been offered a choice of");
                    resMessage.Append(@"currencies for payment, including the");
                    resMessage.Append(@"Merchant's local currency.");
                    resMessage.Append(@"Cardholder agrees to pay the above total");
                    resMessage.Append(@"amount and accept that the selected");
                    resMessage.Append(@"Transaction Currency is final.");
                    resMessage.Append(@"This transaction is based on");
                    resMessage.Append(@"Pure Commerce *Wholesale Rate plus 3.5%");
                    resMessage.Append(@"international Conversion Margin.");
                    resMessage.Append(@"**** CUSTOMER COPY ****");
                    resMessage.Append(@"</CustomerReceipt>");
                    resMessage.Append(@"<MerchantReceipt>");
                    resMessage.Append(@"DATE: 2016-06-25 TIME: 18:06");
                    resMessage.Append(@"MID: 64002711 TID: 42711001");
                    resMessage.Append(@"BATCH: 000416 INVOICE: 4500123");
                    resMessage.Append(@"ECR REF: 9400135000256");
                    resMessage.Append(@"SALE");
                    resMessage.Append(@"CARD NUMBER: 413599******1733");
                    resMessage.Append(@"CARDHOLDER NAME:");
                    resMessage.Append(@"VISA EXPIRY: 0519");
                    resMessage.Append(@"RRN: 180623");
                    resMessage.Append(@"APPROVAL/AUTH CODE:");
                    resMessage.Append(@"APP: VISA DCC CREDIT");
                    resMessage.Append(@"AID: A0000000031010");
                    resMessage.Append(@"TC: B0FE935BE11C80CA");
                    resMessage.Append(@"TOTAL: HKD 125.95");
                    resMessage.Append(@"** DCC TRANSACTION **");
                    resMessage.Append(@"TRANSACTION CURRENCY: USD");
                    resMessage.Append(@"* EXCHANGE RATE: HKD = 0.12975866 USD");
                    resMessage.Append(@"Sale Amt: USD 16.74");
                    resMessage.Append(@"Customer signed electronically");
                    resMessage.Append(@"Cardholder acknowledges that the currency");
                    resMessage.Append(@"conversion is conducted by the Merchant");
                    resMessage.Append(@"and is not associated with or endorsed by");
                    resMessage.Append(@"Visa/MasterCard.");
                    resMessage.Append(@"Cardholder has been offered a choice of");
                    resMessage.Append(@"currencies for payment, including the");
                    resMessage.Append(@"Merchant's local currency.");
                    resMessage.Append(@"Cardholder agrees to pay the above total");
                    resMessage.Append(@"amount and accept that the selected");
                    resMessage.Append(@"Transaction Currency is final.");
                    resMessage.Append(@"This transaction is based on");
                    resMessage.Append(@"Pure Commerce *Wholesale Rate plus 3.5%");
                    resMessage.Append(@"international Conversion Margin.");
                    resMessage.Append(@"**** MERCHANT COPY ****");
                    resMessage.Append(@"</MerchantReceipt>");
                    resMessage.Append(@"</EFTResponse>");

                    resMessage.Insert(0, "  ");
                    resMessage.Insert(0, resMessage.Length.ToString().PadLeft(5, '0'));
                    resMessage.Insert(0, "RA");

                    byte[] res = Encoding.ASCII.GetBytes(resMessage.ToString());
                    handler.Send(res);
                    Console.WriteLine("Sent response: " + resMessage.ToString());

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                    Console.WriteLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
