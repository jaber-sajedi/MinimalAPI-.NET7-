using FluentValidation;
using MinimalAPI_.NET7_.Models.DTO;

namespace MinimalAPI_.NET7_.Validation
{
    public class CouponCreateValidation:AbstractValidator<CouponCreateDTO>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
