﻿using Microsoft.Extensions.Configuration;
using Nexmo.Api;
using System;
using System.Net;

namespace QTimes.api.Infrastructure.NexmoMessaging
{
    public class Messaging: IMessaging
    {
        private readonly string _nexmoApiKey;
        private readonly string _nexmoApiSecret;
        private readonly Client _client;

        public Messaging(IConfiguration configuration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _nexmoApiKey = configuration["Nexmo:Key"];
            _nexmoApiSecret = configuration["Nexmo:Secret"];
            _client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = _nexmoApiKey,
                ApiSecret = _nexmoApiSecret
            });
        }

        public string Send(string from, string toMobileNo, string message)
        {
            string retVal = "";
            try
            {
                message = message.Replace("\\n", " ");

                var results = _client.SMS.Send(new SMS.SMSRequest
                {
                    from = from,
                    to = toMobileNo,
                    text = message
                });

                if (results.messages.Count >= 1)
                {
                    if (results.messages[0].status == "0")
                        retVal = results.messages[0].status.ToString();
                    else
                        retVal = results.messages[0].error_text;
                }

            }
            catch (Exception ex)
            {
                retVal = ex.Message;
            }

            return retVal;
        }
    }
}