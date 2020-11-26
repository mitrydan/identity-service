using System.Collections.Generic;

namespace IdentityService.BlazorClient.Responses
{
    public class BaseListResponse<TModel> : BaseResponse
        where TModel : class
    {
        public IList<TModel> Data { get; set; }
    }
}
