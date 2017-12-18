using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Stepper.StepResult;

namespace Stepper.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            //var runCount = 0;
            //var stepper = new Stepper();
            //stepper.AddFirstStep(new Func<string, StepResult<int>>(test =>
            //{
            //    runCount++;
            //    return StepResult.Success(0);
            //}));

            //stepper.RunJob();
            
        }

        //private static StepResult<int> StepOne(string obj)
        //{
        //    var testObj = 123;
        //    Console.WriteLine($"OutStep One.");
        //    return Success(testObj);
        //}

        //private static StepResult<string> StepTwo(int obj)
        //{
        //    Console.WriteLine($"OutStep Two: {obj}");
        //    return new StepResult<string>()
        //    {
        //        IsSuccess = true,
        //        PassingObj = $"Recieved {obj} from OutStep One!"
        //    };
        //}

        //private static StepResult<CustomClass> StepThree(string someString)
        //{
        //    Console.WriteLine(someString);
        //    var test = new CustomClass()
        //    {
        //        thisString = someString
        //    };

        //    return new StepResult<CustomClass>()
        //    {
        //        IsSuccess = true,
        //        PassingObj = test
        //    };
        //}
    }

    public class CustomClass
    {
        public string thisString { get; set; }
    }
}
