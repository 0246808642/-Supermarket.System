using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Supermercado.Web.Pages;

public class ProdutosModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public ProdutosModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public List<ProductViewModel> Products { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public string? Q { get; set; }

    public int PageSize { get; set; } = 12;
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
                
                // Search filter
                if (!string.IsNullOrWhiteSpace(Q))
                {
                    allProducts = allProducts.Where(p => p.Name.Contains(Q, StringComparison.OrdinalIgnoreCase) || p.Description.Contains(Q, StringComparison.OrdinalIgnoreCase)).ToList();
                }

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
        }
    }
}
