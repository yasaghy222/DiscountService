using DiscountService.Enums;

namespace DiscountService.DTOs;

public class DiscountFilterDto
{
	public DiscountFilterDto() { }

	public DiscountFilterDto(int pageIndex = 1,
										  int pageSize = 10,
										  Guid[]? targetIds = null,
										  DiscountTargetType? targetType = null,
										  DiscountDurationType? durationType = null,
										  DiscountStatus? status = null)
	{
		PageIndex = pageIndex < 0 ? 1 : pageIndex;
		PageSize = pageSize < 0 ? 10 : pageSize;
		TargetType = targetType;
		TargetIds = targetIds;
		DurationType = durationType;
		Status = status;
	}

	public int PageIndex { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public Guid[]? TargetIds { get; set; }
	public DiscountTargetType? TargetType { get; set; }
	public DiscountDurationType? DurationType { get; set; }
	public DiscountStatus? Status { get; set; }
}
