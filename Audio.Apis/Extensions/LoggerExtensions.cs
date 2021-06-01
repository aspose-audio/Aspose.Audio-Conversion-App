using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVedio.Apis
{
    public static class LoggerExtensions
    {
        #region App Domain
        private static readonly Action<ILogger, string, Exception> applicationStarting = LoggerMessage.Define<string>(
            eventId: LoggerEventIds.ApplicationStarting,
            logLevel: LogLevel.Warning,
            formatString: "Starting application ({ApplicationId})");

        private static readonly Action<ILogger, string, Exception> applicationShutdown = LoggerMessage.Define<string>(
            eventId: LoggerEventIds.ApplicationShutdown,
            logLevel: LogLevel.Warning,
            formatString: "Application ({ApplicationId}) shutdown");

        private static readonly Action<ILogger, string, Exception> applicationTerminatedException = LoggerMessage.Define<string>(
            eventId: LoggerEventIds.ApplicationTerminatedException,
            logLevel: LogLevel.Critical,
            formatString: "Application ({ApplicationId}) terminated unexpectedly!");

        private static readonly Action<ILogger, string, Exception> applicationVersion = LoggerMessage.Define<string>(
            eventId: LoggerEventIds.ApplicationInformation,
            logLevel: LogLevel.Warning,
            formatString: "Application Version: {ApplicationVersion}");

        private static readonly Action<ILogger, string, Exception> applicationEnvironment = LoggerMessage.Define<string>(
            eventId: LoggerEventIds.ApplicationInformation,
            logLevel: LogLevel.Warning,
            formatString: "Hosting environment: {ApplicationEnvironment}");
        #endregion

        #region App Domain
        public static void ApplicationStarting(this ILogger logger, string applicationId)
        {
            applicationStarting(logger, applicationId, null);
        }

        public static void ApplicationShutdown(this ILogger logger, string applicationId)
        {
            applicationShutdown(logger, applicationId, null);
        }
        public static void ApplicationTerminatedException(this ILogger logger, string applicationId, Exception exception)
        {
            applicationTerminatedException(logger, applicationId, exception);
        }

        public static void ApplicationError(this ILogger logger, Exception exception, string message, params object[] args)
        {
            logger.ApplicationError(LoggerEventIds.ApplicationError, exception, message, args);
        }

        public static void ApplicationError(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.LogError(eventId, exception, message, args);
        }

        public static void ApplicationVersion(this ILogger logger, string version)
        {
            applicationVersion(logger, version, null);
        }

        public static void ApplicationEnvironment(this ILogger logger, string environment)
        {
            applicationEnvironment(logger, environment, null);
        }

        public static void ApplicationInformation(this ILogger logger, string formatString, params object[] args)
        {
            logger.LogInformation(LoggerEventIds.ApplicationInformation, formatString, args);
        }
        #endregion

        public static string ToMessage(this Exception e,string sessionId) {
            StringBuilder sb = new StringBuilder();
            sb.Append(sessionId);
            sb.Append("-");
            sb.Append(e.Message);
            //sb.Append(Environment.NewLine);
            //sb.Append(e.StackTrace);
            return sb.ToString();
        }
    }

    static class LoggerEventIds
    {
        public static readonly EventId ApplicationStarting = new EventId(1, "ApplicationStarting");
        public static readonly EventId ApplicationShutdown = new EventId(2, "ApplicationShutdown");
        public static readonly EventId ApplicationTerminatedException = new EventId(3, "ApplicationTerminatedException");
        public static readonly EventId ApplicationError = new EventId(4, "ApplicationError");
        public static readonly EventId ApplicationInformation = new EventId(5, "ApplicationInformation");
    }



}
