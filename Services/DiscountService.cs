using System.Linq.Expressions;
using DiscountService.Data;
using DiscountService.DTOs;
using DiscountService.Entities;
using DiscountService.Enums;
using DiscountService.Interfaces;
using DiscountService.Models;
using DiscountService.Shared;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DiscountService;

public class DiscountService(DiscountServiceContext context) : IDiscountService, IDisposable
{
	private readonly DiscountServiceContext _context = context;

	private async Task<Result> Get(Expression<Func<Discount, bool>> predicate)
	{
		Discount? discount = await _context.Discounts.SingleOrDefaultAsync(predicate);
		if (discount == null)
			return CustomErrors.NotFoundData();

		return CustomResults.SuccessOperation(discount);
	}
	public async Task<Result> GetInfo(Guid id)
	{
		Result result = await Get(d => d.Id == id && d.Status == DiscountStatus.Active);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<DiscountInfo>());
		else
			return result;
	}
	public async Task<Result> GetDetail(Guid id)
	{
		Result result = await Get(d => d.Id == id);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<DiscountDetail>());
		else
			return result;
	}

	public async Task<Result> GetAllInfo()
	{
		IQueryable<DiscountInfo> query = from discount in _context.Discounts
										.Where(d => d.Status == DiscountStatus.Active)
										 select new DiscountInfo
										 {
											 Id = discount.Id,
											 Title = discount.Title,
											 Percent = discount.Percent,
											 MinEffectPrice = discount.MinEffectPrice,
											 MaxEffectPrice = discount.MaxEffectPrice,
											 Code = discount.Code,
											 Count = discount.Count,
											 DurationType = discount.DurationType,
											 EndDate = discount.EndDate,
											 StartDate = discount.StartDate
										 };

		return CustomResults.SuccessOperation(await query.ToListAsync());
	}

	public async Task<Result> GetAllDetails(DiscountFilterDto model)
	{
		IQueryable<DiscountDetail> query = from discount in _context.Discounts
										.Skip((model.PageIndex - 1) * model.PageSize)
										.Take(model.PageSize)
										   select new DiscountDetail
										   {
											   Id = discount.Id,
											   Title = discount.Title,
											   Percent = discount.Percent,
											   MinEffectPrice = discount.MinEffectPrice,
											   MaxEffectPrice = discount.MaxEffectPrice,
											   Code = discount.Code,
											   Count = discount.Count,
											   DurationType = discount.DurationType,
											   EndDate = discount.EndDate,
											   StartDate = discount.StartDate,
											   CreateAt = discount.CreateAt,
											   ModifyAt = discount.ModifyAt,
											   Status = discount.Status,
											   TargetId = discount.TargetId,
											   TargetType = discount.TargetType
										   };

		query = model.TargetType switch
		{
			DiscountTargetType.User => query.Where(d => d.TargetType == DiscountTargetType.User && model.TargetIds != null ? model.TargetIds.Any(x => x == d.TargetId) : true),
			DiscountTargetType.Product => query.Where(d => d.TargetType == DiscountTargetType.Product && model.TargetIds != null ? model.TargetIds.Any(x => x == d.TargetId) : true),
			DiscountTargetType.Service => query.Where(d => d.TargetType == DiscountTargetType.Service && model.TargetIds != null ? model.TargetIds.Any(x => x == d.TargetId) : true),
			DiscountTargetType.Public => query.Where(d => d.TargetType == DiscountTargetType.Public),
			_ => query
		};

		query = model.DurationType switch
		{
			DiscountDurationType.Count => query.Where(d => d.DurationType == DiscountDurationType.Count),
			DiscountDurationType.Time => query.Where(d => d.DurationType == DiscountDurationType.Time),
			_ => query
		};

		query = model.Status switch
		{
			DiscountStatus.Inactive => query.Where(d => d.Status == DiscountStatus.Inactive),
			DiscountStatus.Active => query.Where(d => d.Status == DiscountStatus.Active),
			_ => query
		};

		return CustomResults.SuccessOperation(await query.ToListAsync());
	}

	public async Task<Result> CheckCode(string code, Guid targetId)
	{
		Discount? data = await _context.Discounts.SingleOrDefaultAsync(d => d.Code == code && d.Status == DiscountStatus.Active);
		if (data == null)
			return CustomErrors.NotFoundData("کد تخفیف وارد شده صحیح نمی باشد!");

		if (data.Status != DiscountStatus.Active)
			return CustomErrors.NotFoundData("کد تخفیف وارد شده صحیح نمی باشد!");

		if (data.TargetType != DiscountTargetType.Public)
			return data.TargetId == targetId ? CustomResults.SuccessDiscount(data.Adapt<DiscountInfo>()) :
			CustomErrors.InvalidDiscount("کد تخفیف استفاده شده برای این کاربر معتبر نمی باشد!");


		switch (data.DurationType)
		{
			case DiscountDurationType.Time:
				return data.StartDate <= DateTime.Now && data.EndDate >= DateTime.Now ? CustomResults.SuccessDiscount(data.Adapt<DiscountInfo>()) :
				CustomErrors.InvalidDiscount("مدت زمان استفاده از این کد تخفیف به پایان رسیده است.");

			case DiscountDurationType.Count:
				return data.Count >= 1 ? CustomResults.SuccessDiscount(data.Adapt<DiscountInfo>()) :
				 CustomErrors.InvalidDiscount("تعداد قابل استفاده از این کد تخفیف به پایان رسیده است!");
		}

		return CustomResults.SuccessDiscount(data.Adapt<DiscountInfo>());
	}

	public async Task<Result> Add(AddDiscountDto model)
	{
		bool isExist = await _context.Discounts.AnyAsync(s => s.Code == model.Code);
		if (isExist)
			return CustomErrors.InvalidData("کد وارد شد قبلا ثبت شده است!");

		try
		{
			Discount discount = model.Adapt<Discount>();
			await _context.Discounts.AddAsync(discount);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(discount.Adapt<DiscountDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditDiscountDto model)
	{
		Discount? oldData = await _context.Discounts.SingleOrDefaultAsync(d => d.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			oldData = model.Adapt<Discount>();
			_context.Entry(oldData).State = EntityState.Detached;

			_context.Discounts.Update(oldData);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(oldData.Adapt<DiscountInfo>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}
	public async Task<Result> ChangeStatus(Guid id, DiscountStatus status)
	{
		Discount? oldData = await _context.Discounts.SingleOrDefaultAsync(d => d.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Discounts.Where(d => d.Id == id)
														  .ExecuteUpdateAsync(setters => setters.SetProperty(d => d.Status, status));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<DiscountInfo>());
			else
				return CustomErrors.NotFoundData();
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}


	public async Task<Result> Delete(Guid Id)
	{
		try
		{
			int effectedRowCount = await _context.Discounts.Where(x => x.Id == Id).ExecuteDeleteAsync();

			if (effectedRowCount == 1)
				return CustomResults.SuccessDelete();
			else
				return CustomErrors.NotFoundData();
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public void Dispose()
	{
		_context.Dispose();
	}
}
