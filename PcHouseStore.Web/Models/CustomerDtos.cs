namespace PcHouseStore.Web.Models;

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
