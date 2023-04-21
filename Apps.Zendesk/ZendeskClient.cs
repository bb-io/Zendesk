﻿using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk
{
    public class ZendeskClient : RestClient
    {
        public ZendeskClient(string url) : base(new RestClientOptions() { ThrowOnAnyError = true, BaseUrl = new Uri(url) }) { }
    }
}