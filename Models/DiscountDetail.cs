using DiscountService.Entities;
using DiscountService.Enums;

namespace DiscountService.Models;

public class DiscountDetail : BaseEntity
{
	public string? Code { get; set; }
	public required string Title { get; set; }

	public required byte Percent { get; set; }
	public float? MinEffectPrice { get; set; }
	public float? MaxEffectPrice { get; set; }

	public Guid? TargetId { get; set; }
	public DiscountTargetType TargetType { get; set; }

	public DiscountDurationType DurationType { get; set; }
	public int Count { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }

	public DiscountStatus Status { get; set; }
}
