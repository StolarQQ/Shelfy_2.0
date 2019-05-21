using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shelfy.API.Framework.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response,
            int currentPage, int pageSize, int totalCount, int totalPages, string query = "")
        {
            var paginationMetadata = new
            {
                currentPage,
                pageSize,
                totalPages,
                totalCount,
                query
            };

            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            response.Headers.Add("Pagination",
                JsonConvert.SerializeObject(paginationMetadata, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}