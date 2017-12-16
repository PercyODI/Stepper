using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class StepResult<T> : IResult, IStepResult
    {
        public bool EndJob { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public T PassingObj { get; set; }
        public bool HasPassingObj
        {
            get { return true; }
            set { }
        }

    }

    public class StepResult : IStepResult
    {
        public bool EndJob { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public bool HasPassingObj
        {
            get { return false; }
            set { }
        }

        public static StepResult<T> Success<T>(T passingObj = default(T))
        {
            return new StepResult<T>()
            {
                EndJob = false,
                IsSuccess = true,
                PassingObj = passingObj
            };
        }

        public static StepResult Failure()
        {
            return new StepResult()
            {
                EndJob = true,
                IsSuccess = false
            };
        }
    }
}