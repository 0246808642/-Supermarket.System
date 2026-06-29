using Supermercado.Application.DTOs.Address;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;

namespace Supermercado.Application.Services;

public class AddressAppService : IAddressAppService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public AddressAppService(IAddressRepository addressRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _addressRepository = addressRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> AddAddressAsync(CreateAddressDto input)
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var address = new Address(_currentUser.UserId.Value, input.Street, input.Number, input.City, input.State, input.ZipCode);
        _addressRepository.Add(address);
        
        await _unitOfWork.CommitAsync();
        return address.Id;
    }

    public async Task<IEnumerable<AddressOutputDto>> GetMyAddressesAsync()
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var addresses = await _addressRepository.GetByCustomerIdAsync(_currentUser.UserId.Value);
        return addresses.Select(a => new AddressOutputDto(a.Id, a.Street, a.Number, a.City, a.State, a.ZipCode));
    }

    public async Task RemoveAddressAsync(Guid id)
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var address = await _addressRepository.GetByIdAsync(id);
        if (address == null || address.CustomerId != _currentUser.UserId.Value)
            throw new Exception("Endereço não encontrado.");
            
        _addressRepository.Remove(address);
        await _unitOfWork.CommitAsync();
    }
}
