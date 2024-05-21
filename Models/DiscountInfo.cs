using DiscountService.Enums;

namespace DiscountService.Models;

public class DiscountInfo
{
	public Guid Id { get; set; }
	public string? Code { get; set; }
	public required string Title { get; set; }

	public required byte Percent { get; set; }
	public float? MinEffectPrice { get; set; }
	public float? MaxEffectPrice { get; set; }

	public DiscountDurationType DurationType { get; set; }
	public int Count { get; set; } = 1;
	public DateTime? StartDate { get; set; } = DateTime.Now;
	public DateTime? EndDate { get; set; }
}
