using DiscountService.DTOs;
using DiscountService.Enums;
using DiscountService.Models;

namespace DiscountService.Interfaces;

public interface IDiscountService
{
	Task<Result> GetInfo(Guid id);
	Task<Result> GetDetail(Guid id);
	Task<Result> CheckCode(string code, Guid targetId);

	Task<Result> GetAllInfo();
	Task<Result> GetAllDetails(DiscountFilterDto model);

	Task<Result> Add(AddDiscountDto model);
	Task<Result> Edit(EditDiscountDto model);
	Task<Result> ChangeStatus(Guid id, DiscountStatus status);

	Task<Result> Delete(Guid id);
}
