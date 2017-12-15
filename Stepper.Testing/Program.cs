using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var stepper = new Stepper<string>();
            stepper
                .AddFirstStep(new Step<string, int>(StepOne))
                .Then(new Step<int, string>(StepTwo));
            stepper.RunJob();
            Console.ReadLine();
        }

        public static StepResult<int> StepOne(string obj)
        {
            var testObj = 123;
            Console.WriteLine($"Step One: {obj?.GetHashCode()}");
            return new StepResult<int>()
            {
                IsSuccess = true,
                PassingObj = testObj
            };
        }

        public static StepResult<string> StepTwo(int obj)
        {
            Console.WriteLine($"Step Two: {obj}");
            return new StepResult<string>()
            {
                IsSuccess = true
            };
        }
    }
}
