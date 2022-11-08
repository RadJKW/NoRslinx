﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using NoRslinx.Application.Common.Interfaces;

namespace NoRslinx.Application.Common.Behaviours;
public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;


    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;

    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;


        _logger.LogInformation("NoRslinx Request: {Name} {@Request}",
            requestName, request);

        await Task.CompletedTask;
    }
}