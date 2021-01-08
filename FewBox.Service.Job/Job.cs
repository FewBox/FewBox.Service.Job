using System;
using Microsoft.Extensions.Logging;
using FewBox.Core.Utility.Net;
using System.Collections.Generic;
using FewBox.Core.Utility.Formatter;

namespace FewBox.Service.Job
{
    public class Job : IJob
    {
        private IList<EndpointEvent> EndpointEvents { get; set; }
        private ILogger Logger { get; set; }

        public Job(IList<EndpointEvent> endpointEvents, ILogger<Job> logger)
        {
            this.EndpointEvents = endpointEvents;
            this.Logger = logger;
        }

        public void Execute()
        {
            try
            {
                foreach (var endpointEvent in this.EndpointEvents)
                {
                    string url = $"{endpointEvent.Endpoint.Protocol}://{endpointEvent.Endpoint.Host}:{endpointEvent.Endpoint.Port}/{endpointEvent.Event.Path}";
                    this.Logger.LogDebug(url);
                    dynamic response;
                    dynamic body = JsonUtility.Deserialize<dynamic>(endpointEvent.Event.Body);
                    switch (endpointEvent.Event.EnumType)
                    {
                        case EventType.Get:
                            response = RestfulUtility.Get<dynamic>(url, new List<Header> { });
                            break;
                        case EventType.Post:
                            response = RestfulUtility.Post<dynamic, dynamic>(url, new Package<dynamic>
                            {
                                Headers = new List<Header> { },
                                Body = body
                            });
                            break;
                        case EventType.Put:
                            response = RestfulUtility.Put<dynamic, dynamic>(url, new Package<dynamic>
                            {
                                Headers = new List<Header> { },
                                Body = body
                            });
                            break;
                        case EventType.Patch:
                            response = RestfulUtility.Patch<dynamic, dynamic>(url, new Package<dynamic>
                            {
                                Headers = new List<Header> { },
                                Body = body
                            },
                            "application/json-patch+json");
                            break;
                        case EventType.Delete:
                            response = RestfulUtility.Delete<dynamic>(url, new List<Header> { });
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception.Message);
            }
        }
    }
}