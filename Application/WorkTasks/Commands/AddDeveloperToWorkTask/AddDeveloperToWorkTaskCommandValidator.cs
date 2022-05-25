using FluentValidation;

namespace Application.WorkTasks.Commands.AddDeveloperToWorkTask;

public class AddDeveloperToWorkTaskCommandValidator : AbstractValidator<AddDeveloperToWorkTaskCommand>
{
    public AddDeveloperToWorkTaskCommandValidator()
    {
        RuleFor(x => x.DeveloperId)
            .NotEmpty()
            .NotNull();
    }
}