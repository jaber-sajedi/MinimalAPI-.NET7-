using Microsoft.AspNetCore.Mvc;
using MinimalAPI_.NET7_.Data;
using MinimalAPI_.NET7_.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/api/coupon/", () =>
{
  return  Results.Ok(CouponStore.couponList);
}).WithName("GetCoupons");
 app.MapGet("/api/coupon/{id:int}",(int id) =>
{
    return Results.Ok(CouponStore.couponList.FirstOrDefault(c => c.Id == id));
}).WithName("GetCoupon"); ;


app.MapPost("/api/coupon/", ([FromBody] Coupon coupun) =>
{
    if (coupun.Id!=0 || string.IsNullOrEmpty(coupun.Name))
    {
        return Results.BadRequest("Invalid Id or Coupon Name");
    }

    if (CouponStore.couponList.FirstOrDefault(c=>c.Name.ToLower()==coupun.Name.ToLower())!=null)
    {
        return Results.BadRequest("coupn Name already Exist");
    }
    coupun.Id = CouponStore.couponList.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupun);
    return Results.CreatedAtRoute("GetCoupon", new {id=coupun.Id}, coupun);
    //return Results.Created($"/api/coupon/{coupun.Id}", coupun);
}).WithName("CreateCoupon"); ;

//app.MapPost("/api/coupon/", () =>
//{

//});
//app.MapDelete("/api/coupon/{id:int}", (int id) =>
//{

//});

app.UseHttpsRedirection();
app.Run();

