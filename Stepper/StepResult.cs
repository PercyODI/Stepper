using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class StepResult<T> : IStepResult
    {
        public bool EndJob { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public T PassingObject { get; set; }
        public bool HasPassingObj
        {
            get { return true; }
            set {  }
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

        public static StepResult<T> Success<T>(T passingObj)
        {
            return new StepResult<T>()
            {
                EndJob = false,
                IsSuccess = true,
                PassingObject = passingObj
            };
        }

        public static StepResult Success()
        {
            return new StepResult()
            {
                EndJob = false,
                IsSuccess = true
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