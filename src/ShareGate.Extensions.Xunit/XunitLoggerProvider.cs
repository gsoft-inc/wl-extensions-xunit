using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ShareGate.Extensions.Xunit;

internal sealed class XunitLoggerProvider : ILoggerProvider, ILogger
{
    private static readonly Dictionary<LogLevel, string> LogLevelStrings = new Dictionary<LogLevel, string>
    {
        [LogLevel.None] = "NON",
        [LogLevel.Trace] = "TRC",
        [LogLevel.Debug] = "DBG",
        [LogLevel.Information] = "INF",
        [LogLevel.Warning] = "WRN",
        [LogLevel.Error] = "ERR",
        [LogLevel.Critical] = "CRT",
    };

    public ITestOutputHelper? Output { get; set; }

    public ILogger CreateLogger(string categoryName)
    {
        return this;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (this.Output != null)
        {
            var message = formatter(state, exception);
            this.Output.WriteLine("[{0:HH:mm:ss:ffff} {1}] {2}", DateTime.Now, LogLevelStrings[logLevel], message);
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return new NoopDisposable();
    }

    public void Dispose()
    {
    }

    private sealed class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}