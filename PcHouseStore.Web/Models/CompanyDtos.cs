namespace PcHouseStore.Web.Models;

public record CompanyResponse(
    long CompanyId,
    string LegalName,
    string? TradingName,
    string? VatNumber,
    string? RegistrationNumber,
    string? Email,
    string? PhonePrimary,
    string? PhoneSecondary,
    string? Website,
    long? BillingAddressId,
    long? ShippingAddressId,
    DateTime CreatedAt,
    string? BillingAddress,
    string? ShippingAddress,
    string Name);

public record CreateCompanyRequest(
    string LegalName,
    string? TradingName,
    string? VatNumber,
    string? RegistrationNumber,
    string? Email,
    string? PhonePrimary,
    string? PhoneSecondary,
    string? Website,
    long? BillingAddressId,
    long? ShippingAddressId);

public record UpdateCompanyRequest(
    string LegalName,
    string? TradingName,
    string? VatNumber,
    string? RegistrationNumber,
    string? Email,
    string? PhonePrimary,
    string? PhoneSecondary,
    string? Website,
    long? BillingAddressId,
    long? ShippingAddressId);
