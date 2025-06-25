using FluentValidation;

namespace OrdersProject.Application.Features.Orders.Queries.GetPageable;

public class GetPageableOrdersQueryValidator : AbstractValidator<GetPageableOrdersQuery>
{
    public GetPageableOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Numer strony musi być większy niż 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Rozmiar strony musi być między 1 a 100.");
    }
}
