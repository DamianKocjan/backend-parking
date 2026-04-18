using AppCore.Dtos;
using AppCore.Models;
using FluentValidation;

namespace AppCore.Validators;

public class ParkingGateValidator : AbstractValidator<CreateGateDto>
{
    public ParkingGateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa bramik jest wymagana.")
            .MaximumLength(20).WithMessage("Nazwa nie może przekraczać 20 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwa zawiera niedozwolone znaki.");

        RuleFor(x => x.Location)
            .MaximumLength(50).WithMessage("Lokalizacja nie może przekraczać 50 znaków.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Lokalizacja zawiera niedozwolone znaki.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Typ bramki jest wymagany.")
            .Must(type => Enum.TryParse<GateType>(type, out _))
            .WithMessage("Nieprawidłowy typ bramki. Dozwolone wartości: Entry, Exit, Both.");
    }
}