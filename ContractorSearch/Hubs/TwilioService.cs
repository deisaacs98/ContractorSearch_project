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
            ApiKeys apiKeys = new ApiKeys();


            TwilioClient.Init(apiKeys.TwilioAccountSid, apiKeys.TwilioToken);

            var message = MessageResource.Create(
                body: messageToSend,
                from: new Twilio.Types.PhoneNumber(apiKeys.TwilioFrom),
                to: new Twilio.Types.PhoneNumber(apiKeys.TwilioTo)
            );

            Console.WriteLine(message.Sid);
        }
    }
}
