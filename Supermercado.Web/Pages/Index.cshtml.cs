using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Supermercado.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public IndexModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public List<ProductViewModel> Products { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient("SupermercadoApi");
            var response = await client.GetAsync("/api/Products");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var allProducts = JsonSerializer.Deserialize<List<ProductViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductViewModel>();
                
                // Pagination logic
                TotalPages = (int)Math.Ceiling(allProducts.Count / (double)PageSize);
                
                if (CurrentPage < 1) CurrentPage = 1;
                if (CurrentPage > TotalPages && TotalPages > 0) CurrentPage = TotalPages;

                Products = allProducts
                            .Skip((CurrentPage - 1) * PageSize)
                            .Take(PageSize)
                            .ToList();
            }
        }
        catch (Exception)
        {
            // Omit error handling for brevity, could show a toast message
        }
    }
}

public class ProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
}
