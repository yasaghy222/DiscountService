using DiscountService.Models;

namespace DiscountService.Shared;

public class CustomErrors
{
	public static Result InvalidData(object? errors = null) => new()
	{
		Message = new()
		{
			Fa = "داده های وارد شده معتبر نمی باشد",
			En = "Data is Invalid!"
		},
		StatusCode = StatusCodes.Status400BadRequest,
		Data = errors,
		Status = false
	};

	public static Result InvalidDiscount(object? errors = null) => new()
	{
		Message = new()
		{
			Fa = "کد تخفیف وارد شده معتبر نیست!",
			En = "Data is Invalid!"
		},
		StatusCode = StatusCodes.Status400BadRequest,
		Data = errors,
		Status = false
	};


	public static Result InternalServer(object? errors = null) => new()
	{
		Message = new()
		{
			Fa = "با عرض پوزش، عملیات درخواستی با مشکل مواجه شد",
			En = "Operation Failed!"
		},
		StatusCode = StatusCodes.Status500InternalServerError,
		Data = errors,
		Status = false
	};


	public static Result NotFoundData(object? errors = null) => new()
	{
		Message = new()
		{
			Fa = "داده ای با مشخصات وارد شده یافت نشد",
			En = "Data not Found!"
		},
		StatusCode = StatusCodes.Status404NotFound,
		Data = errors,
		Status = false
	};

}
