using DiscountService.DTOs;
using FluentValidation;

namespace DoctorService.Validations;

public class AddDiscountValidation : AbstractValidator<AddDiscountDto>
{
	public AddDiscountValidation()
	{
		RuleFor(s => s.Title).NotNull()
							 .NotEmpty()
							 .MaximumLength(100);

		RuleFor(s => s.Code).MaximumLength(15);

		RuleFor(s => s.Percent).NotEmpty().NotNull()
		.ExclusiveBetween(1, 100)
		.WithMessage("درصد تخفیف باید بین ۱ تا ۱۰۰ درصد باشد!");

		RuleFor(op => op.MinEffectPrice).GreaterThanOrEqualTo(0)
		.WithMessage("حداقل مبلغ اعمال تخفیف باید بزرگتر از صفر باشد!")
		.LessThanOrEqualTo(x => x.MaxEffectPrice)
		.WithMessage("حداقل مبلغ تخفیف باید کوچکتر یا مساوی حداکثر مبلغ اعمال تخفیف باشد!");

		RuleFor(op => op.MaxEffectPrice).GreaterThanOrEqualTo(0)
		.WithMessage("حداکثر مبلغ اعمال تخفیف باید بزرگتر از صفر باشد!")
		.GreaterThanOrEqualTo(x => x.MinEffectPrice)
		.WithMessage("حداکثر مبلغ اعمال تخفیف باید بزرگتر یا برابر حداقل مبلغ اعمال تخفیف باشد!");

		RuleFor(op => op.Count).GreaterThanOrEqualTo(0)
		.WithMessage("تعداد موجودی کد تخفیف نمی تواند کمتر از صفر باشد!");

		RuleFor(op => op.StartDate).NotNull()
										 .NotEmpty()
										 .GreaterThanOrEqualTo(op => DateTime.Now)
										 .WithMessage("تاریخ شروع نمی تواند کوچکتر از تاریخ الان باشد!");

		RuleFor(op => op.EndDate).NotNull()
								 .NotEmpty()
								 .GreaterThan(op => op.StartDate)
								 .WithMessage("تاریخ پایانی نمی تواند کوچکتر از تاریخ شروع باشد!");
	}
}
