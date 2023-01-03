using System.Collections;
using System.Net;
using Newtonsoft.Json;

namespace Contract
{
    public class ApiResponseBase
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set;}
        public dynamic Data { get; set; }
        
        public HttpStatusCode StatusCode { get; set; }
    }
    
    public class ApiIdentityResponse : ApiResponseBase
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set;}
    }
}