using System.Linq;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

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

public static class CompanyMapper
{
    public static CompanyResponse ToResponse(Company company)
    {
        var billingAddress = company.BillingAddress != null
            ? FormatAddress(company.BillingAddress)
            : null;

        var shippingAddress = company.ShippingAddress != null
            ? FormatAddress(company.ShippingAddress)
            : null;

        return new CompanyResponse(
            company.CompanyId,
            company.LegalName,
            company.TradingName,
            company.VatNumber,
            company.RegistrationNumber,
            company.Email,
            company.PhonePrimary,
            company.PhoneSecondary,
            company.Website,
            company.BillingAddressId,
            company.ShippingAddressId,
            company.CreatedAt,
            billingAddress,
            shippingAddress,
            company.TradingName ?? company.LegalName);
    }

    private static string FormatAddress(Address address)
    {
        var parts = new[]
        {
            address.Line1,
            address.Line2,
            address.City,
            address.County,
            address.Postcode
        };

        return string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}
