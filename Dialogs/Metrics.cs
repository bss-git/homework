using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialogs
{
    public static class Metrics
    {
        public static readonly Counter MessagesTotal = Prometheus.Metrics.CreateCounter("dialogs_messages_total", "Total number of sent messages");

        public static readonly Counter RequestsTotal = Prometheus.Metrics.CreateCounter("dialogs_requests_total", "Total number of incoming requests");

        public static readonly Counter ErrorsTotal = Prometheus.Metrics.CreateCounter("dialogs_errors_total", "Total number of errors");

        public static readonly Histogram RequestDuration = Prometheus.Metrics.CreateHistogram("dialogs_request_duration", "Histogram of request processing durations");
    }
}
