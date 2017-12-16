using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class StepResult<T> : IResult
    {
        public bool EndJob { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public T PassingObj { get; set; }
    }

    public static class StepResult
    {
        public static StepResult<T> Success<T>(T passingObj = default(T))
        {
            return new StepResult<T>()
            {
                EndJob = false,
                IsSuccess = true,
                PassingObj = passingObj
            };
        }

        public static StepResult<T> Failure<T>()
        {
            return new StepResult<T>()
            {
                EndJob = true,
                IsSuccess = false
            };
        }
    }
}
