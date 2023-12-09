using FluentValidation;
using MinimalAPI_.NET7_.Models.DTO;

namespace MinimalAPI_.NET7_.Validation
{
    public class CouponUpdateValidation:AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
