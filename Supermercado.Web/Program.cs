var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient("SupermercadoApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5031");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/api/search", async (string? q, IHttpClientFactory clientFactory) => 
{
    if (string.IsNullOrWhiteSpace(q)) return Results.Ok(new List<object>());

    var client = clientFactory.CreateClient("SupermercadoApi");
    var response = await client.GetAsync("/api/Products");
    
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        var allProducts = System.Text.Json.JsonSerializer.Deserialize<List<Supermercado.Web.Pages.ProductViewModel>>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        
        var filtered = allProducts
            .Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
            .Take(5)
            .Select(p => new { p.Id, p.Name, p.ImageUrl, p.Price, p.DiscountedPrice })
            .ToList();
            
        return Results.Ok(filtered);
    }
    
    return Results.Ok(new List<object>());
});

app.Run();
