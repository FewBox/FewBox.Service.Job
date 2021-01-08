using System;

namespace FewBox.Service.Job
{
    public class Event
    {
        public string Path { get; set; }
        public string Type { get; set; }
        public EventType EnumType
        {
            get
            {
                if (Enum.TryParse(this.Type, out EventType eventType))
                {
                    return eventType;
                }
                else
                {
                    return EventType.Get;
                }
            }
        }
        public string Body { get; set; }
    }
}