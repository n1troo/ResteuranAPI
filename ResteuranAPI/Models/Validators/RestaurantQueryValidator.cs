using FluentValidation;

using ResteuranAPI.Entities;

namespace ResteuranAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {

        private int[] _allowedPageSizes = new[] { 5, 10, 20, 50 };
        private string[] _allowedColumnNames = new[] { nameof(Restaurant.Name), nameof(Restaurant.Description), nameof(Restaurant.Category) };

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

            RuleFor(s => s.SortBy).Custom((value, context) =>
            {
                if (!_allowedColumnNames.Contains(value) || string.IsNullOrEmpty(value))
                {
                    context.AddFailure("SortBy", $"Wrong SortBy column!");
                }
            });

        }
    }
}
