namespace Web.Api.Common.Http
{
    public class HttpResponseJson
    {
        public bool success { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public string date { get; set; }
        public int total { get; set; }
        public string time { get; set; }


    }
}
