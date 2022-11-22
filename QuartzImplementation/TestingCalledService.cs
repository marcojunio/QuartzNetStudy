namespace QuartzImplementation;

public class TestingCalledService
{
    public async Task CalledHttpClientTesting()
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        
        if(response.IsSuccessStatusCode)
            Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}