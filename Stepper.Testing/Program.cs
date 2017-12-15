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
            var stepper = new Stepper();
            stepper.AddStep(new Step(StepOne)).Then(new Step(StepTwo));
            stepper.RunJob();
            Console.ReadLine();
        }

        public static StepResult StepOne(object obj)
        {
            var testObj = new Tuple<int, string>(123, "456");
            Console.WriteLine($"Step One: {obj?.GetHashCode()}");
            return new StepResult()
            {
                IsSuccess = true,
                PassingObj = testObj
            };
        }

        public static StepResult StepTwo(object obj)
        {
            Tuple<int, string> test = obj as Tuple<int, string>;
            Console.WriteLine($"Step Two: {test?.Item1}");
            return new StepResult()
            {
                IsSuccess = true
            };
        }
    }
}
