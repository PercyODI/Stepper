using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Step<T, U>
    {
        private StepOptions Options { get; set; }
        private Func<T, StepResult<T>> StepFunc { get; set; }
        internal Step<T> NextStep { get; set; }
        public Step(Func<T, StepResult<T>> stepFunc, StepOptions options = null)
        {
            if (options == null)
                options = new StepOptions();

            Options = options;
            StepFunc = stepFunc;
        }

        public Step<U> Then<U>(Step<U> nextStep)
        {
            NextStep = nextStep;
            return NextStep;
        }

        internal void RunStep(JobResult jobResult)
        {
            if (jobResult.HasFailed && !Options.AlwaysRun)
            {
                NextStep?.RunStep(jobResult);
                return;
            }

            StepResult<T> stepResult;
            try
            {
                stepResult = StepFunc();
            }
            catch (Exception ex)
            {
                stepResult = new StepResult<T>()
                {
                    IsSuccess = false,
                    Exception = ex
                };
            }

            if (!stepResult.IsSuccess && Options.StopJobOnError)
            {
                jobResult.HasFailed = true;
            }
        }

        public static implicit operator Step<T>(Step<object> v)
        {
            throw new NotImplementedException();
        }
    }

    public class StepOptions
    {
        public bool StopJobOnError = true;
        public bool AlwaysRun = false;
    }
}
