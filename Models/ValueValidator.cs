using FluentValidation;

namespace TestTask.Models
{
    public class ValueValidator : AbstractValidator<Value>
    {
        public ValueValidator() {

            RuleFor(x => x.Date).LessThan(DateTime.Now).GreaterThan(new DateTime(2020,1,1)).NotNull();
            RuleFor(x => x.Second).GreaterThan(-1).LessThan(84600).NotNull();
            RuleFor(x => x.Indicator).GreaterThan(-1).NotNull();
        }








    }
}
