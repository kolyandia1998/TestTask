using FluentValidation;

namespace TestTask.Models
{
    public class ValueValidator : AbstractValidator<Value>
    {
        public ValueValidator() {
            RuleFor(x => x.StartTime).LessThan(DateTime.Now).GreaterThan(new DateTime(2020,1,1)).NotNull();
            RuleFor(x => x.CompletionTime).GreaterThan(-1).LessThan(84600).NotNull();
            RuleFor(x => x.Index).GreaterThan(-1).NotNull();
        }








    }
}
