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

public class CreateOrderRequest
{
    public long CompanyId { get; set; }
    public long? CustomerId { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string? Currency { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TotalVatAmount { get; set; }
    public decimal BalanceDue { get; set; }
    public string? Notes { get; set; }
    public long CreatedByEmployeeId { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public long? DeviceId { get; set; }
    public long? TechnicianId { get; set; }
    public string? CheckInNotes { get; set; }
    public DateTime? EstimatedCompletion { get; set; }
    public DateTime? PlacedAt { get; set; }
    public int? WarrantyDays { get; set; }
    public OrderStatus? Status { get; set; }
    public List<OrderLineRequest> Lines { get; set; } = new();
}

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

public class UpdateOrderRequest
{
    public OrderStatus? Status { get; set; }
    public decimal? BalanceDue { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Notes { get; set; }
}
