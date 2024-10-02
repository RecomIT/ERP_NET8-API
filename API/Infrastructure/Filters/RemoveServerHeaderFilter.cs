using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace API.Infrastructure.Filters
{
    public class RemoveServerHeaderFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers.Remove("Server");
            await next();
        }
    }
}
