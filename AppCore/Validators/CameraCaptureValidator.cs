using AppCore.Dtos;
using FluentValidation;

namespace AppCore.Validators;

public class CameraCaptureValidator : AbstractValidator<CameraCaptureDto>
{
    public CameraCaptureValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Numer rejestracyjny jest wymagany.")
            .MaximumLength(15).WithMessage("Numer rejestracyjny nie może przekraczać 15 znaków.")
            .Matches(@"^[A-Z0-9\s\-]+$").WithMessage("Numer rejestracyjny zawiera niedozwolone znaki.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Marka pojazdu jest wymagana.");
        
        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Marka pojazdu jest wymagana.");
        
        RuleFor(x => x.GateName)
            .NotEmpty().WithMessage("Nazwa bramki jest wymagana.")
            .MaximumLength(20).WithMessage("Nazwa bramki nie może przekraczać 20 znaków.");
    }
}