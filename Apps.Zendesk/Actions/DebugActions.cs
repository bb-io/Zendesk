﻿using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Actions;

[ActionList]
public class DebugActions(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    [Action("[DEBUG] Action", Description = "Action for debugging purposes")]
    public List<AuthenticationCredentialsProvider> Debug()
    {
        return InvocationContext.AuthenticationCredentialsProviders.ToList();
    }
}