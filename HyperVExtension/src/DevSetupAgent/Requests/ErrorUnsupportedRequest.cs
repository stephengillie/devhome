﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json.Nodes;

namespace HyperVExtension.DevSetupAgent;

/// <summary>
/// Class used to handle unsupported requests.
/// </summary>
internal sealed class ErrorUnsupportedRequest : RequestBase
{
    public ErrorUnsupportedRequest(IRequestContext requestContext)
        : base(requestContext)
    {
    }

    public override bool IsStatusRequest => true;

    public override IHostResponse Execute(ProgressHandler progressHandler, CancellationToken stoppingToken)
    {
        return new ErrorUnsupportedRequestResponse(RequestId, RequestType);
    }
}
