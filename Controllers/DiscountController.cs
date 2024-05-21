using DiscountService.Data;
using DiscountService.DTOs;
using DiscountService.Enums;
using DiscountService.Models;
using DiscountService.Shared;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace DiscountService.Controllers;

[ApiController]
[Route("[controller]")]
public class DiscountController(DiscountServiceContext context,
							  IValidator<AddDiscountDto> addValidator,
							  IValidator<EditDiscountDto> editValidator) : ControllerBase
{
	private readonly DiscountService _service = new(context);
	readonly IValidator<AddDiscountDto> _addValidator = addValidator;
	readonly IValidator<EditDiscountDto> _editValidator = editValidator;

	[HttpGet]
	[Route("/[controller]/{type}/{id}")]
	public async Task<IActionResult> Get(GetDiscountType type, Guid id)
	{
		Result result = type switch
		{
			GetDiscountType.Info => await _service.GetInfo(id),
			GetDiscountType.Detail => await _service.GetDetail(id),
			_ => await _service.GetInfo(id)
		};
		return StatusCode(result.StatusCode, result.Data);
	}
	[HttpGet]
	[Route("/[controller]/{type}")]
	public async Task<IActionResult> Get(GetDiscountType type,
																	[FromQuery] int pageIndex = 1,
																	[FromQuery] int pageSize = 10,
																	[FromQuery] Guid[]? targetIds = null,
																	[FromQuery] DiscountTargetType? targetType = null,
																	[FromQuery] DiscountDurationType? durationType = null,
																	[FromQuery] DiscountStatus? status = null)
	{
		DiscountFilterDto filterDto = new(pageIndex, pageSize, targetIds, targetType, durationType, status);

		Result result = type switch
		{
			GetDiscountType.Info => await _service.GetAllInfo(),
			GetDiscountType.Detail => await _service.GetAllDetails(filterDto),
			_ => await _service.GetAllInfo()
		};
		return StatusCode(result.StatusCode, result.Data);
	}


	[HttpGet]
	[Route("/[controller]/CheckCode/{code}/{targetId}")]
	public async Task<IActionResult> Get(string code, Guid targetId)
	{
		Result result = await _service.CheckCode(code, targetId);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(EditDiscountDto model)
	{
		Result result;
		ValidationResult validationResult = _editValidator.Validate(model);

		if (!validationResult.IsValid)
			result = CustomErrors.InvalidData(validationResult.Errors);
		else
			result = await _service.Edit(model);

		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(AddDiscountDto model)
	{
		Result result;
		ValidationResult validationResult = _addValidator.Validate(model);

		if (!validationResult.IsValid)
			result = CustomErrors.InvalidData(validationResult.Errors);
		else
			result = await _service.Add(model);

		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/Status/{id}")]
	public async Task<IActionResult> Patch(Guid id, DiscountStatus status)
	{
		Result result = await _service.ChangeStatus(id, status);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpDelete]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		Result result = await _service.Delete(id);
		return StatusCode(result.StatusCode, result.Data);
	}
}
