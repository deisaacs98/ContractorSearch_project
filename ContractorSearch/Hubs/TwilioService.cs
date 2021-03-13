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
        //There was a merge conflict here. ApiKeys should be static,
        //so will be leaving like this.
        //
        //        public void SendText(string messageToSend)
        //        {
        //            ApiKeys apiKeys = new ApiKeys();


        //            TwilioClient.Init(apiKeys.TwilioAccountSid, apiKeys.TwilioToken);

        //            var message = MessageResource.Create(
        //                body: messageToSend,
        //                from: new Twilio.Types.PhoneNumber(apiKeys.TwilioFrom),
        //                to: new Twilio.Types.PhoneNumber(apiKeys.TwilioTo)
        //            );
        //
        public void SendText(string messageToSend)
        {
            TwilioClient.Init(ApiKeys.TwilioAccountSid, ApiKeys.TwilioToken);

            var message = MessageResource.Create(
                body: messageToSend,
                from: new Twilio.Types.PhoneNumber(ApiKeys.TwilioFrom),
                to: new Twilio.Types.PhoneNumber(ApiKeys.TwilioTo)
            );
        }
    }
}
