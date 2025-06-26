using FluentValidation;

namespace OrdersProject.Application.Features.Orders.Queries.GetById;

public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID jest wymagane.");
    }
}
