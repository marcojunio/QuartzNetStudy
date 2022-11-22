using System.Text;
using Newtonsoft.Json;

namespace QuartzImplementation;

public class Middleware
{
    private readonly RequestDelegate _next;

    public Middleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBody = context.Response.Body;
        try
        {
            await using var memStream = new MemoryStream();
            
            context.Response.Body = memStream;
            await _next(context);

            memStream.Position = 0;
            
            var responseBody = new StreamReader(memStream).ReadToEndAsync();

            var data = JsonConvert.DeserializeObject(await responseBody);
            
            var json = new
            {
                data, 
            };

            var jsonSerialized = JsonConvert.SerializeObject(json);

            var buffer = Encoding.UTF8.GetBytes(jsonSerialized);
            
            await using var output = new MemoryStream(buffer);
            
            await output.CopyToAsync(originalBody);
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }
}