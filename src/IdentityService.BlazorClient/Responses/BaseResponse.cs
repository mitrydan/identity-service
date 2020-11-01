namespace IdentityService.BlazorClient.Responses
{
    public abstract class BaseResponse
    {
        public bool IsFailed { get; set; }

        public int? HttpStatusCode { get; set; }
    }
}
