using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Web.Models;

public record OrderResponse(
    long OrderId,
    string OrderNumber,
    OrderType OrderType,
    OrderStatus Status,
    string Currency,
    decimal TotalAmount,
    decimal TotalVatAmount,
    decimal BalanceDue,
    DateTime PlacedAt,
    DateTime? ClosedAt,
    string? Notes,
    long CompanyId,
    long? CustomerId,
    string? CustomerName,
    long CreatedByEmployeeId,
    string? CreatedByName,
    SalesChannel? SalesChannel,
    ServiceOrderDetails? ServiceDetails,
    IReadOnlyCollection<OrderLineResponse> Lines,
    IReadOnlyCollection<OrderStatusEventResponse>? StatusHistory,
    IReadOnlyCollection<OrderNoteResponse>? NotesHistory,
    IReadOnlyCollection<PaymentResponse>? Payments,
    IReadOnlyCollection<RefundResponse>? Refunds);

public record OrderLineResponse(
    long OrderLineId,
    long CatalogItemId,
    long? RefurbItemId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal VatRate,
    decimal VatAmount,
    decimal LineTotal,
    OrderFulfilmentStatus FulfilmentStatus);

public record OrderStatusEventResponse(
    long OrderStatusEventId,
    OrderStatus Status,
    DateTime ChangedAt,
    string? Comment,
    long EmployeeId,
    string? EmployeeName);

public record OrderNoteResponse(
    long OrderNoteId,
    long EmployeeId,
    string? EmployeeName,
    string Note,
    DateTime CreatedAt);

public record PaymentResponse(
    long PaymentId,
    PaymentType PaymentType,
    decimal Amount,
    DateTime ProcessedAt,
    long EmployeeId,
    string? EmployeeName,
    PayMethod? MethodCode);

public record RefundResponse(
    long RefundId,
    decimal Amount,
    RefundReason Reason,
    DateTime ProcessedAt,
    long EmployeeId,
    string? EmployeeName);

public record ServiceOrderDetails(
    long? DeviceId,
    string? DeviceBrand,
    string? DeviceModel,
    string? DeviceSerial,
    long? TechnicianId,
    string? TechnicianName,
    string? CheckInNotes,
    DateTime? EstimatedCompletion,
    DateTime? CompletedAt,
    DateTime? PickedUpAt,
    int WarrantyDays);

public record CreateOrderRequest(
    long CompanyId,
    long? CustomerId,
    string OrderType,
    string OrderNumber,
    string? Currency,
    decimal TotalAmount,
    decimal TotalVatAmount,
    decimal BalanceDue,
    string? Notes,
    long CreatedByEmployeeId,
    SalesChannel? SalesChannel,
    long? DeviceId,
    long? TechnicianId,
    string? CheckInNotes,
    DateTime? EstimatedCompletion,
    DateTime? PlacedAt,
    int? WarrantyDays,
    OrderStatus? Status,
    List<OrderLineRequest> Lines);

public record OrderLineRequest(
    long CatalogItemId,
    string Description,
    decimal Quantity,
    decimal UnitPrice,
    decimal VatRate,
    decimal VatAmount,
    decimal LineTotal,
    long? RefurbItemId,
    OrderFulfilmentStatus? FulfilmentStatus);

public record UpdateOrderRequest(
    OrderStatus? Status,
    decimal? BalanceDue,
    DateTime? ClosedAt,
    string? Notes);
