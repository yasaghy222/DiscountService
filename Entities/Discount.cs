using DiscountService.Enums;

namespace DiscountService.Entities
{
    public class Discount : BaseEntity
    {
        public string? Code { get; set; }
        public required string Title { get; set; }

        public required byte Percent { get; set; }
        public float? MinEffectPrice { get; set; }
        public float? MaxEffectPrice { get; set; }

        public Guid? TargetId { get; set; }
        public DiscountTargetType TargetType { get; set; } = DiscountTargetType.Public;

        public DiscountDurationType DurationType { get; set; } = DiscountDurationType.Count;
        public int Count { get; set; } = 1;
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }

        public DiscountStatus Status { get; set; } = DiscountStatus.Active;
    }
}