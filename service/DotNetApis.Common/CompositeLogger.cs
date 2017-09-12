﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nito.Disposables;

namespace DotNetApis.Common
{
    /// <summary>
    /// A logger that writes messages to a collection of loggers.
    /// </summary>
    public sealed class CompositeLogger : ILogger
    {
        private readonly IEnumerable<ILogger> _loggers;

        public CompositeLogger(IEnumerable<ILogger> loggers)
        {
            _loggers = loggers.Where(x => x != null).ToList();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => _loggers.Do(x => x.Log(logLevel, eventId, state, exception, formatter));

        public bool IsEnabled(LogLevel logLevel) => _loggers.Any(x => x.IsEnabled(logLevel));

        public IDisposable BeginScope<TState>(TState state) => CollectionDisposable.Create(_loggers.Select(x => x.BeginScope(state)));
    }
}