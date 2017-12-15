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
        private Func<object, StepResult> StepFunc { get; set; }
        internal Step NextStep { get; set; }
        public Step(Func<object, StepResult> stepFunc, StepOptions options = null)
        {
            if (options == null)
                options = new StepOptions();

            Options = options;
            StepFunc = stepFunc;
        }

        public Step Then(Step nextStep)
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

            StepResult stepResult;
            try
            {
                stepResult = StepFunc(jobResult.StepResults.LastOrDefault()?.PassingObj);
            }
            catch (Exception ex)
            {
                stepResult = new StepResult()
                {
                    IsSuccess = false,
                    Exception = ex
                };
            }

            if (!stepResult.IsSuccess && Options.StopJobOnError)
            {
                jobResult.HasFailed = true;
            }

            jobResult.StepResults.Add(stepResult);
            NextStep?.RunStep(jobResult);
        }
    }

    public class StepOptions
    {
        public bool StopJobOnError = true;
        public bool AlwaysRun = false;
    }
}
