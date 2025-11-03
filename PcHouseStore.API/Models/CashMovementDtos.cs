using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

public record CashMovementResponse(
    long CashMovementId,
    long CompanyId,
    long EmployeeId,
    CashMovementType MovementType,
    decimal Amount,
    string? Reason,
    long? RelatedPaymentId,
    DateTime OccurredAt,
    string? EmployeeName);

public record CreateCashMovementRequest(
    long CompanyId,
    long EmployeeId,
    CashMovementType MovementType,
    decimal Amount,
    string? Reason,
    long? RelatedPaymentId);

public record UpdateCashMovementRequest(
    CashMovementType? MovementType,
    decimal? Amount,
    string? Reason);

public static class CashMovementMapper
{
    public static CashMovementResponse ToResponse(CashMovement cashMovement)
    {
        return new CashMovementResponse(
            cashMovement.CashMovementId,
            cashMovement.CompanyId,
            cashMovement.EmployeeId,
            cashMovement.MovementType,
            cashMovement.Amount,
            cashMovement.Reason,
            cashMovement.RelatedPaymentId,
            cashMovement.OccurredAt,
            cashMovement.Employee?.Person.DisplayName);
    }
}
