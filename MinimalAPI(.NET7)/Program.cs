using System.Net;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MinimalAPI_.NET7_;
using MinimalAPI_.NET7_.Data;
using MinimalAPI_.NET7_.Models;
using MinimalAPI_.NET7_.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency injection

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
 
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Get

app.MapGet("/api/coupon/", (ILogger<Program> _logger) =>
{
    APIResponse response = new();
    _logger.Log(LogLevel.Information, "Getting all Coupons");
    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("GetCoupons").Produces<APIResponse>(200); ;


app.MapGet("/api/coupon/{id:int}", (ILogger<Program> _logger, int id) =>
{
    APIResponse response = new();
    response.Result = CouponStore.couponList.FirstOrDefault(c => c.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("GetCoupon").Produces<APIResponse>(200);


#endregion

#region Post

app.MapPost("/api/coupon/", async (IMapper _mapper,IValidator<CouponCreateDTO> _validation,[FromBody] CouponCreateDTO coupun_C_DTO) =>
{

    APIResponse response = new(){IsSuccess = false,StatusCode = HttpStatusCode.BadRequest};

    var validationResult =await _validation.ValidateAsync(coupun_C_DTO);
    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add( validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    if (CouponStore.couponList.FirstOrDefault(c => c.Name.ToLower() == coupun_C_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("coupn Name already Exist");
        return Results.BadRequest(response);
    }

    Coupon coupun = _mapper.Map<Coupon>(coupun_C_DTO);
    coupun.Id = CouponStore.couponList.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupun);
    CouponDTO couponDto = _mapper.Map<CouponDTO>(coupun);
   
    response.Result = couponDto;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;
    return Results.Ok(response);
    // return Results.CreatedAtRoute("GetCoupon", new { id = coupun.Id }, couponDto );
    //return Results.Created($"/api/coupon/{coupun.Id}", coupun);
}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

#endregion


#region Put

app.MapPut("/api/coupon/", async (IMapper _mapper, IValidator<CouponUpdateDTO> _validation, [FromBody] CouponUpdateDTO coupun_U_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    var validationResult = await _validation.ValidateAsync(coupun_U_DTO);
    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(c => c.Id == coupun_U_DTO.Id);
    couponFromStore.Name = coupun_U_DTO.Name;
    couponFromStore.IsActive = coupun_U_DTO.IsActive;
    couponFromStore.Percent = coupun_U_DTO.Percent;
    couponFromStore.LastUpdated = DateTime.Now;

    response.Result = _mapper.Map<CouponDTO>(couponFromStore);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);

}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400);

#endregion


app.MapDelete("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(c => c.Id == id);

    if (couponFromStore!=null)
    {
        CouponStore.couponList.Remove(couponFromStore);
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(response);
    }
});

app.UseHttpsRedirection();
app.Run();

