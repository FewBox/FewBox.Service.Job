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
                    object response;
                    dynamic body = JsonUtility.Deserialize<object>(endpointEvent.Event.Body);
                    if (endpointEvent.JWTToken != null && endpointEvent.JWTToken.Length > 0)
                    {
                        switch (endpointEvent.Event.EnumType)
                        {
                            case EventType.Get:
                                response = RestfulUtility.Get<object>(url, endpointEvent.JWTToken, new List<Header> { });
                                break;
                            case EventType.Post:
                                response = RestfulUtility.Post<object, object>(url, endpointEvent.JWTToken, new Package<dynamic>
                                {
                                    Headers = new List<Header> { },
                                    Body = body
                                });
                                break;
                            case EventType.Put:
                                response = RestfulUtility.Put<object, object>(url, endpointEvent.JWTToken, new Package<dynamic>
                                {
                                    Headers = new List<Header> { },
                                    Body = body
                                });
                                break;
                            case EventType.Patch:
                                response = RestfulUtility.Patch<object, object>(url, endpointEvent.JWTToken, new Package<dynamic>
                                {
                                    Headers = new List<Header> { },
                                    Body = body
                                },
                                "application/json-patch+json");
                                break;
                            case EventType.Delete:
                                response = RestfulUtility.Delete<object>(url, endpointEvent.JWTToken, new List<Header> { });
                                break;
                            default:
                                response = new Object { };
                                break;
                        }
                    }
                    else
                    {
                        switch (endpointEvent.Event.EnumType)
                        {
                            case EventType.Get:
                                response = RestfulUtility.Get<object>(url, new List<Header> { });
                                break;
                            case EventType.Post:
                                response = RestfulUtility.Post<object, object>(url, new Package<dynamic>
                                {
                                    Headers = new List<Header> { },
                                    Body = body
                                });
                                break;
                            case EventType.Put:
                                response = RestfulUtility.Put<object, object>(url, new Package<dynamic>
                                {
                                    Headers = new List<Header> { },
                                    Body = body
                                });
                                break;
                            case EventType.Patch:
                                response = RestfulUtility.Patch<object, object>(url, new Package<dynamic>
                                {
                                    Headers = new List<Header> { },
                                    Body = body
                                },
                                "application/json-patch+json");
                                break;
                            case EventType.Delete:
                                response = RestfulUtility.Delete<object>(url, new List<Header> { });
                                break;
                            default:
                                response = new Object { };
                                break;
                        }
                    }
                    string responseString = JsonUtility.Serialize(response);
                    this.Logger.LogInformation(responseString);
                }

            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception.Message);
            }
        }
    }
}