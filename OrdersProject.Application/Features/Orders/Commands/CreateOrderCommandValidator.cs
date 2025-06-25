using FluentValidation;

namespace OrdersProject.Application.Features.Orders.Commands;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.SourceEmail)
            .NotEmpty().EmailAddress().WithMessage("Nieprawidłowy adres e-mail.");

        When(x => !x.IsFromEmail, () =>
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Nazwa klienta jest wymagana.");

            RuleFor(x => x.Items)
                .NotNull().NotEmpty().WithMessage("Zamówienie musi zawierać co najmniej jeden produkt.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductName).NotEmpty();
                item.RuleFor(i => i.Quantity).GreaterThan(0);
                item.RuleFor(i => i.Price).GreaterThanOrEqualTo(0);
            });
        });
    }
}
