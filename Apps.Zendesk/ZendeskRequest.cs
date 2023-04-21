using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk
{
    public class ZendeskRequest : RestRequest
    {
        public ZendeskRequest(string endpoint, Method method, AuthenticationCredentialsProvider provider) : base(endpoint, method)
        {
            this.AddHeader("Authorization", provider.Value);
            this.AddHeader("accept", "*/*");
        }
    }
}
