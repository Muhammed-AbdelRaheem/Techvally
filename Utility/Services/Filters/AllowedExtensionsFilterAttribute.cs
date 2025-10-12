using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Utility.Services.Filters;


public static class StringExtensions
{
    public static string ToSlug(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        // Replace spaces with hyphens
        var slug = value.ToLower().Replace(" ", "-");

        // Remove invalid chars
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");

        return slug;
    }
}
public class AllowedExtensionsFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string[]? _extensions = null;
        private readonly ILogger<AllowedExtensionsFilterAttribute> _logger;

        public AllowedExtensionsFilterAttribute(string[]? extensions = null, ILogger<AllowedExtensionsFilterAttribute> logger = null)
        {
            _extensions = extensions;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var file = (IFormFile)context.ActionArguments["file"];
            if (file != null)
            {

                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    context.Result = new NotFoundResult();
                }
                else
                {
                    if (extension.Contains("svg"))
                    {
                        try
                        {
                            using (var reader = new StreamReader(file.OpenReadStream()))
                            {
                                var ss = await reader.ReadToEndAsync();
                                
                                if (ss.Contains("<script") || ss.Contains("</script>"))
                                {
                                    context.Result = new BadRequestObjectResult(new { status = 404, message = $"file not valid", });
                                }
                                else
                                {
                                    await next();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            context.Result = new BadRequestObjectResult(new { status = 404, message = $"catch file not valid {ex.Message}", });
                        }
                    }
                    else
                    {
                        await next();
                    }
                }
            }
            else
            {
                context.Result = new NotFoundResult();

            }
        }


     
    }
