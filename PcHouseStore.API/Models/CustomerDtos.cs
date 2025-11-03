using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record CustomerResponse(
    long CustomerId,
    long PersonId,
    long? CompanyId,
    bool MarketingOptIn,
    DateTime CreatedAt,
    string? PersonName,
    string? PersonEmail,
    string? PersonPhone);

public record CreateCustomerRequest(
    long PersonId,
    long? CompanyId,
    bool MarketingOptIn);

public record UpdateCustomerRequest(
    long? CompanyId,
    bool MarketingOptIn);

public static class CustomerMapper
{
    public static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse(
            customer.CustomerId,
            customer.PersonId,
            customer.CompanyId,
            customer.MarketingOptIn,
            customer.CreatedAt,
            customer.Person?.DisplayName,
            customer.Person?.Email,
            customer.Person?.PhoneMobile ?? customer.Person?.PhoneHome);
    }
}
