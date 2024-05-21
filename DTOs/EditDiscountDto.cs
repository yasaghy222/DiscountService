using DiscountService.Enums;

namespace DiscountService.DTOs;

public class EditDiscountDto
{
	public Guid Id { get; set; }
	public string? Code { get; set; }
	public required string Title { get; set; }

	public required int Percent { get; set; }
	public float? MinEffectPrice { get; set; }
	public float? MaxEffectPrice { get; set; }

	public Guid? Target { get; set; }
	public DiscountTargetType TargetType { get; set; } = DiscountTargetType.Public;

	public DiscountDurationType DurationType { get; set; } = DiscountDurationType.Count;
	public int Count { get; set; } = 1;
	public DateTime? StartDate { get; set; } = DateTime.Now;
	public DateTime? EndDate { get; set; }
}
