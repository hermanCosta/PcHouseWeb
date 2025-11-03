using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record PaymentDetailResponse(
    long PaymentId,
    long CompanyId,
    long? OrderId,
    long PaymentMethodId,
    PaymentType PaymentType,
    decimal Amount,
    decimal NetCash,
    decimal NetCard,
    decimal NetVoucher,
    DateTime ProcessedAt,
    long EmployeeId,
    string? Reference,
    string? Notes,
    string? PaymentMethodCode,
    string? EmployeeName);

public record CreatePaymentRequest(
    long CompanyId,
    long? OrderId,
    long PaymentMethodId,
    PaymentType PaymentType,
    decimal Amount,
    decimal NetCash,
    decimal NetCard,
    decimal NetVoucher,
    long EmployeeId,
    string? Reference,
    string? Notes);

public record UpdatePaymentRequest(
    PaymentType? PaymentType,
    decimal? Amount,
    string? Reference,
    string? Notes);

public static class PaymentMapper
{
    public static PaymentDetailResponse ToResponse(Payment payment)
    {
        return new PaymentDetailResponse(
            payment.PaymentId,
            payment.CompanyId,
            payment.OrderId,
            payment.PaymentMethodId,
            payment.PaymentType,
            payment.Amount,
            payment.NetCash,
            payment.NetCard,
            payment.NetVoucher,
            payment.ProcessedAt,
            payment.EmployeeId,
            payment.Reference,
            payment.Notes,
            payment.PaymentMethod?.MethodCode.ToString(),
            payment.Employee?.Person.DisplayName);
    }
}
