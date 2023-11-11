using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace VoterApp.Application.Common.PipelineBehaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _timer;

    private const int ElapsedMsWarningThreshold = 500;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestGuid = Guid.NewGuid().ToString();

        _logger.LogInformation("Handling request {RequestGuid} {@Request}", requestGuid, request);

        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMs = _timer.ElapsedMilliseconds;

        if (elapsedMs <= ElapsedMsWarningThreshold)
        {
            _logger.LogInformation("Request {RequestGuid} {RequestName} finished after {ElapsedMs} ms", requestGuid, requestName, elapsedMs);
        }
        else
        {
            _logger.LogWarning("Long Running Request {RequestGuid} {RequestName} finished after {ElapsedMs} ms", requestGuid, requestName, elapsedMs);
        }

        return response;
    }
}