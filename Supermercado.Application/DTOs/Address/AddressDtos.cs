namespace Supermercado.Application.DTOs.Address;

public record CreateAddressDto(string Street, string Number, string City, string State, string ZipCode);
public record UpdateAddressDto(string Street, string Number, string City, string State, string ZipCode);
public record AddressOutputDto(Guid Id, string Street, string Number, string City, string State, string ZipCode);
