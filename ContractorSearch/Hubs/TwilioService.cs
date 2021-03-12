using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace ContractorSearch.Hubs
{
    public class TwilioService
    {
        public void SendText(string messageToSend)
        { 


            TwilioClient.Init(ApiKeys.TwilioAccountSid, ApiKeys.TwilioToken);

            var message = MessageResource.Create(
                body: messageToSend,
                from: new Twilio.Types.PhoneNumber(ApiKeys.TwilioFrom),
                to: new Twilio.Types.PhoneNumber(ApiKeys.TwilioTo)
            );

            Console.WriteLine(message.Sid);
        }
    }
}
