namespace FewBox.Service.Job
{
    public class EndpointEvent
    {
        public string JWTToken { get; set; }
        public Endpoint Endpoint { get; set; }
        public Event Event { get; set; }
    }
}