using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PcHouseStore.Domain.Enums;

namespace PcHouseStore.Domain.Models;

[Table("service_order")]
public class ServiceOrder : Order
{
    public ServiceOrder()
    {
        OrderType = OrderType.Service;
    }

    [Column("device_id")]
    public long? DeviceId { get; set; }

    [Column("technician_id")]
    public long? TechnicianId { get; set; }

    [Column("check_in_notes")]
    public string? CheckInNotes { get; set; }

    [Column("estimated_completion")]
    public DateTime? EstimatedCompletion { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Column("picked_up_at")]
    public DateTime? PickedUpAt { get; set; }

    [Column("warranty_days")]
    public int WarrantyDays { get; set; } = 90;

    public Device? Device { get; set; }
    public Employee? Technician { get; set; }
}