using System.Net;

namespace IdentityService.BlazorClient.Responses
{
    public abstract class BaseResponse
    {
        public bool IsFailed { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }
    }
}
