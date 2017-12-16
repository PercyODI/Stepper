using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Step<InT, OutT> : IStep<InT>
    {
        private StepOptions Options { get; set; }
        private Func<InT, IStepResult> StepFunc { get; set; }
        private IStep<OutT> NextStep { get; set; }

        public Step(Func<InT, IStepResult> stepFunc, StepOptions options = null)
        {
            if (options == null)
                options = new StepOptions();

            Options = options;
            StepFunc = stepFunc;
        }

        public Step<NewIT, NewOT> Then<NewIT, NewOT>(Step<NewIT, NewOT> nextStep)
            where NewIT : OutT
        {
            NextStep = nextStep as IStep<OutT>;
            return NextStep as Step<NewIT, NewOT>;
        }

        public Step<NewIT, NewOT> Then<NewIT, NewOT>(Func<NewIT, IStepResult> newStepFunc)
            where NewIT : OutT
        {
            var nextStep = new Step<NewIT, NewOT>(newStepFunc);
            return Then<NewIT, NewOT>(nextStep);
        }

        public void RunStep(JobResult jobResult, InT passedObj)
        {
            if (jobResult.HasFailed && !Options.AlwaysRun)
            {
                NextStep?.RunStep(jobResult, default(OutT));
                return;
            }

            IStepResult stepResult;
            try
            {
                stepResult = StepFunc(passedObj);
            }
            catch (Exception ex)
            {
                stepResult = new StepResult<OutT>()
                {
                    IsSuccess = false,
                    EndJob = true,
                    Exception = ex
                };
            }

            if (stepResult.EndJob && Options.StopJobOnError)
            {
                jobResult.HasFailed = true;
            }

            NextStep?.RunStep(jobResult,
                stepResult.GetType() == typeof(StepResult<OutT>) ? ((StepResult<OutT>) stepResult).PassingObj : default(OutT));
        }
    }

    public class StepOptions
    {
        public bool StopJobOnError = true;
        public bool AlwaysRun = false;
    }
}