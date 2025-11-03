using System.Linq;
using PcHouseStore.Domain.Enums;
using PcHouseStore.Domain.Models;

namespace PcHouseStore.API.Models;

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

public record AddOrderNoteRequest(string Note);

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

public static class OrderFactory
{
    public static Order CreateOrder(CreateOrderRequest request)
    {
        if (!Enum.TryParse<OrderType>(request.OrderType, true, out var parsedType))
        {
            throw new ArgumentException($"Unsupported order type '{request.OrderType}'", nameof(request));
        }

        return parsedType switch
        {
            OrderType.Retail or OrderType.Refurb => CreateRetailOrder(request, parsedType),
            OrderType.Service => CreateServiceOrder(request),
            _ => throw new NotSupportedException($"Order type '{parsedType}' is not supported yet")
        };
    }

    private static Order CreateRetailOrder(CreateOrderRequest request, OrderType parsedType)
    {
        var retail = new RetailOrder
        {
            SalesChannel = request.SalesChannel ?? SalesChannel.InStore,
        };

        if (parsedType == OrderType.Refurb)
        {
            retail.OrderType = OrderType.Refurb;
        }

        return MapCommonOrderProperties(retail, request);
    }

    private static Order CreateServiceOrder(CreateOrderRequest request)
    {
        var service = new ServiceOrder
        {
            DeviceId = request.DeviceId,
            TechnicianId = request.TechnicianId,
            CheckInNotes = request.CheckInNotes,
            EstimatedCompletion = request.EstimatedCompletion,
            WarrantyDays = request.WarrantyDays ?? 90,
        };

        return MapCommonOrderProperties(service, request);
    }

    private static Order MapCommonOrderProperties(Order order, CreateOrderRequest request)
    {
        order.CompanyId = request.CompanyId;
        order.CustomerId = request.CustomerId;
        order.OrderNumber = string.IsNullOrWhiteSpace(request.OrderNumber)
            ? GenerateOrderNumber(request.OrderType)
            : request.OrderNumber;
        order.Currency = request.Currency ?? order.Currency;
        order.TotalAmount = request.TotalAmount;
        order.TotalVatAmount = request.TotalVatAmount;
        order.BalanceDue = request.BalanceDue;
        order.Notes = request.Notes;
        order.PlacedAt = request.PlacedAt ?? DateTime.UtcNow;
        order.CreatedByEmployeeId = request.CreatedByEmployeeId;
        order.Status = request.Status ?? OrderStatus.Created;

        foreach (var line in request.Lines)
        {
            order.Lines.Add(new OrderLine
            {
                CatalogItemId = line.CatalogItemId,
                RefurbItemId = line.RefurbItemId,
                Description = line.Description,
                Quantity = line.Quantity,
                UnitPrice = line.UnitPrice,
                VatRate = line.VatRate,
                VatAmount = line.VatAmount,
                LineTotal = line.LineTotal,
                FulfilmentStatus = line.FulfilmentStatus ?? OrderFulfilmentStatus.Pending
            });
        }

        return order;
    }

    private static string GenerateOrderNumber(string orderType)
    {
        var prefix = orderType[..Math.Min(3, orderType.Length)].ToUpperInvariant();
        return $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}

public static class OrderMapper
{
    public static OrderResponse ToResponse(Order order, bool includeDetails = false)
    {
        var lines = order.Lines
            .Select(line => new OrderLineResponse(
                line.OrderLineId,
                line.CatalogItemId,
                line.RefurbItemId,
                line.Description,
                line.Quantity,
                line.UnitPrice,
                line.VatRate,
                line.VatAmount,
                line.LineTotal,
                line.FulfilmentStatus))
            .ToList();

        var statusHistory = includeDetails
            ? order.StatusHistory
                .OrderByDescending(s => s.ChangedAt)
                .Select(s => new OrderStatusEventResponse(
                    s.OrderStatusEventId,
                    s.Status,
                    s.ChangedAt,
                    s.Comment,
                    s.ChangedByEmployeeId,
                    s.ChangedBy?.Person.DisplayName))
                .ToList()
            : new List<OrderStatusEventResponse>();

        var notes = includeDetails
            ? order.NotesHistory
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new OrderNoteResponse(
                    n.OrderNoteId,
                    n.EmployeeId,
                    n.Employee?.Person.DisplayName,
                    n.Note,
                    n.CreatedAt))
                .ToList()
            : new List<OrderNoteResponse>();

        var payments = includeDetails
            ? order.Payments
                .OrderByDescending(p => p.ProcessedAt)
                .Select(p => new PaymentResponse(
                    p.PaymentId,
                    p.PaymentType,
                    p.Amount,
                    p.ProcessedAt,
                    p.EmployeeId,
                    p.Employee?.Person.DisplayName,
                    p.PaymentMethod?.MethodCode))
                .ToList()
            : new List<PaymentResponse>();

        var refunds = includeDetails
            ? order.Refunds
                .OrderByDescending(r => r.ProcessedAt)
                .Select(r => new RefundResponse(
                    r.RefundId,
                    r.Amount,
                    r.ReasonCode,
                    r.ProcessedAt,
                    r.ProcessedByEmployeeId,
                    r.ProcessedBy?.Person.DisplayName))
                .ToList()
            : new List<RefundResponse>();

        SalesChannel? salesChannel = order is RetailOrder retail ? retail.SalesChannel : null;
        ServiceOrderDetails? serviceDetails = null;

        if (order is ServiceOrder service)
        {
            serviceDetails = new ServiceOrderDetails(
                service.DeviceId,
                service.Device?.Brand,
                service.Device?.Model,
                service.Device?.SerialNumber,
                service.TechnicianId,
                service.Technician?.Person.DisplayName,
                service.CheckInNotes,
                service.EstimatedCompletion,
                service.CompletedAt,
                service.PickedUpAt,
                service.WarrantyDays);
        }

        return new OrderResponse(
            order.OrderId,
            order.OrderNumber,
            order.OrderType,
            order.Status,
            order.Currency,
            order.TotalAmount,
            order.TotalVatAmount,
            order.BalanceDue,
            order.PlacedAt,
            order.ClosedAt,
            order.Notes,
            order.CompanyId,
            order.CustomerId,
            order.Customer?.Person.DisplayName,
            order.CreatedByEmployeeId,
            order.CreatedBy?.Person.DisplayName,
            salesChannel,
            serviceDetails,
            lines,
            includeDetails ? statusHistory : null,
            includeDetails ? notes : null,
            includeDetails ? payments : null,
            includeDetails ? refunds : null);
    }
}
