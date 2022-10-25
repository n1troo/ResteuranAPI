using FluentValidation;
using ResteuranAPI.Entities;

namespace ResteuranAPI.Models.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
{
    private readonly RestaurantDbContext _restaurantDbContext;

    public RegisterUserValidator(RestaurantDbContext restaurantDbContext)
    {
        _restaurantDbContext = restaurantDbContext;
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(s => s.Password)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(s => s.ConfirmPassword).Matches(a => a.Password);

        RuleFor(s => s.Email)
            .Custom((value, context) =>
            {
                var result = _restaurantDbContext.Users.Any(s => s.Email == value);
                if (result)
                {
                    context.AddFailure("There is user with this email adress!");
                };
            });
    }
}