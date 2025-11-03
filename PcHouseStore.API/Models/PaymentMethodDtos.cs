using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record PaymentMethodResponse(
    long PaymentMethodId,
    long CompanyId,
    PayMethod MethodCode,
    string? Description,
    bool IsActive);

public record CreatePaymentMethodRequest(
    long CompanyId,
    PayMethod MethodCode,
    string? Description,
    bool IsActive);

public record UpdatePaymentMethodRequest(
    PayMethod? MethodCode,
    string? Description,
    bool? IsActive);

public static class PaymentMethodMapper
{
    public static PaymentMethodResponse ToResponse(PaymentMethod paymentMethod)
    {
        return new PaymentMethodResponse(
            paymentMethod.PaymentMethodId,
            paymentMethod.CompanyId,
            paymentMethod.MethodCode,
            paymentMethod.Description,
            paymentMethod.IsActive);
    }
}
