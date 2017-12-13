using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Step
    {
        private StepOptions Options { get; set; }
        private Func<JobResult, StepResult> StepFunc { get; set; }
        public Step(Func<JobResult, StepResult> stepFunc, StepOptions options = null)
        {
            if (options == null)
                options = new StepOptions();

            Options = options;
            StepFunc = stepFunc;
        }

        internal StepResult RunStep(JobResult jobResult)
        {
            if (jobResult == null)
                 throw new NotImplementedException();

            StepResult stepResult;
            try
            {
                stepResult = StepFunc(jobResult);
            }
            catch (Exception ex)
            {
                stepResult = new StepResult()
                {
                    IsSuccess = false,
                    Exception = ex
                };
            }

            return stepResult;
        }
    }

    public class StepOptions
    {
        public bool StopJobOnError = true;
        public bool AlwaysRun = false;
    }
}
