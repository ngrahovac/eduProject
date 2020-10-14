using eduProjectModel.Input;
using FluentValidation;

namespace eduProjectWebAPI.Validation
{
    public class CollaboratorProfileInputValidator : AbstractValidator<CollaboratorProfileInputModel>
    {
        public CollaboratorProfileInputValidator()
        {
            RuleFor(collaborator => collaborator.CollaboratorProfileType)
                .NotEmpty().WithMessage("Polje ne može biti prazno.");

            RuleFor(collaborator => collaborator.FacultyName)
                .NotEmpty().WithMessage("Polje ne može biti prazno.");

            RuleFor(collaborator => collaborator.Cycle)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Polje ne može biti prazno.")
                .InclusiveBetween(1, 3).WithMessage("Ciklus ne postoji");

            RuleFor(collaborator => collaborator.StudyProgramName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Polje ne može biti prazno")
                .Length(2, 200).WithMessage("Nevalidan unos");

            RuleFor(collaborator => collaborator.StudyYear)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Polje ne može biti prazno")
                .InclusiveBetween(1, 8).WithMessage("Godina ne postoji");

            RuleFor(collaborator => collaborator.StudyFieldName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Polje ne može biti prazno")
                .Length(2, 200).WithMessage("Nevalidan unos");

            RuleFor(collaborator => collaborator.StudyProgramSpecializationName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Polje ne može biti prazno")
                .Length(2, 200).WithMessage("Nevalidan unos");

            RuleFor(collaborator => collaborator.ActivityDescription)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Length(200).WithMessage("Maksimalna dužina unosa prekoračena.");
        }
    }
}
