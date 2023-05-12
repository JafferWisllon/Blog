using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Api.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErros(this ModelStateDictionary modelState)
        {
            var result = new List<string>();
            foreach (var item in modelState.Values)
                result.AddRange(item.Errors.Select(x => x.ErrorMessage));
 
            return result;
        }
    }
}
