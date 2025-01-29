using FluentValidation;

namespace ClinicalTrials.Api.Validators;

public record UploadRequest(IFormFile File);

public class UploadRequestValidator : AbstractValidator<UploadRequest>
{
    public UploadRequestValidator()
    {
        RuleFor(r => r.File)
            .NotEmpty()
            .WithMessage("File must be provided.")
            .Must(f => f.ContentType.Contains("application/json"))
            .WithMessage("Must be a JSON file.")
            .Must(f => f.Length < 256)
            .WithMessage("The file must not be longer than 256 bytes.");
    }
}
