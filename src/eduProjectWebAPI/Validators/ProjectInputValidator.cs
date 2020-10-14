using eduProjectModel.Input;
using FluentValidation;
using System;

namespace eduProjectWebAPI.Validation
{
    public class ProjectInputValidator : AbstractValidator<ProjectInputModel>
    {
        public ProjectInputValidator()
        {
            RuleFor(project => project.Title)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .Length(2, 200).WithMessage("Nevalidan unos");

            RuleFor(project => project.StudyFieldName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .Length(2, 200).WithMessage("Nevalidan unos");

            RuleFor(project => project.StartDate)
                .NotNull().WithMessage("Datum mora biti unesen")
                .GreaterThan(DateTime.Now).WithMessage("Nevalidan datum");

            RuleFor(project => project.EndDate)
                .NotNull().WithMessage("Datum mora biti unesen")
                .GreaterThan(project => project.StartDate).WithMessage("Nevalidan datum");

            RuleFor(project => project.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Polje ne može biti prazno")
                .Length(2, 200).WithMessage("Maksimalna dužina unosa prekoračena");

            RuleFor(project => project.TagNames)
                .NotNull()
                .NotEmpty().WithMessage("Barem jedan tag mora biti odabran");

            RuleFor(project => project.CollaboratorProfileInputModels)
                .NotNull()
                .NotEmpty().WithMessage("Barem jedan kolaborator mora biti potreban");

            RuleForEach(project => project.CollaboratorProfileInputModels)
                .SetValidator(new CollaboratorProfileInputValidator());
        }
    }
}
