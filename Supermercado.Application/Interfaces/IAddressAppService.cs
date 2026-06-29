using Supermercado.Application.DTOs.Address;

namespace Supermercado.Application.Interfaces;

public interface IAddressAppService
{
    Task<Guid> AddAddressAsync(CreateAddressDto input);
    Task<IEnumerable<AddressOutputDto>> GetMyAddressesAsync();
    Task RemoveAddressAsync(Guid id);
}
