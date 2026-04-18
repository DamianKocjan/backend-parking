using AppCore.Dtos;
using FluentValidation;

namespace AppCore.Validators;

public class ParkingTariffValidator : AbstractValidator<CreateTariffDto>
{
    public ParkingTariffValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa taryfy jest wymagana.")
            .MaximumLength(50).WithMessage("Nazwa nie może przekraczać 50 znaków.");
        
        RuleFor(x => x.FreeMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Darmowe minuty muszą być większe lub równe 0.");
        
        RuleFor(x => x.HourlyRate)
            .GreaterThanOrEqualTo(0).WithMessage("Stawka godzinowa musi być większa lub równa 0.");
        
        RuleFor(x => x.DailyMaxRate)
            .GreaterThanOrEqualTo(0).WithMessage("Maksymalna stawka dzienna musi być większa lub równa 0.")
            .GreaterThanOrEqualTo(x => x.HourlyRate).WithMessage("Maksymalna stawka dzienna musi być większa lub równa stawce godzinowej.");
    }
}