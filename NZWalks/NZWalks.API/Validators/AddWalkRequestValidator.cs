using FluentValidation;

namespace NZWalks.API.Validators
{
    public class AddWalkRequestValidator : AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);

            // referenced ids are checked in the validation menthods
        }
    }
}
