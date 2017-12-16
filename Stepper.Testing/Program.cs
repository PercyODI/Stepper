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
                .AddFirstStep<string, int>(StepOne)
                .Then<int, string>(StepTwo)
                .Then<string, CustomClass>(StepThree);
            stepper.RunJob();
            Console.ReadLine();
        }

        private static StepResult<int> StepOne(string obj)
        {
            var testObj = 123;
            Console.WriteLine($"Step One.");
            return new StepResult<int>()
            {
                IsSuccess = true,
                PassingObj = testObj
            };
        }

        private static StepResult<string> StepTwo(int obj)
        {
            Console.WriteLine($"Step Two: {obj}");
            return new StepResult<string>()
            {
                IsSuccess = true,
                PassingObj = $"Recieved {obj} from Step One!"
            };
        }

        private static StepResult<CustomClass> StepThree(string someString)
        {
            Console.WriteLine(someString);
            var test = new CustomClass()
            {
                thisString = someString
            };

            return new StepResult<CustomClass>()
            {
                IsSuccess = true,
                PassingObj = test
            };
        }
    }

    public class CustomClass
    {
        public string thisString { get; set; }
    }
}
