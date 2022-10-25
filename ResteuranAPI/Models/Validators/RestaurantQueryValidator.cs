using FluentValidation;

namespace ResteuranAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
       
        private int[] _allowedPageSizes = new[] { 5, 10, 20, 50 };

        public RestaurantQueryValidator()
        {

            RuleFor(s => s.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(s => s.PageSize)
                .Custom((value, context) =>
                {
                    if (!_allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"Wrong page size! {context.DisplayName}");
                    }
                });

        }
    }
}
    