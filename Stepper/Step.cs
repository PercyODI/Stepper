using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Step<InT, OutT> : IStep<InT>
    {
        private Type _inputType;
        private StepOptions Options { get; set; }
        private Func<InT, StepResult<OutT>> StepFunc { get; set; }
        internal IStep<OutT> NextStep { get; set; }
        public Step(Func<InT, StepResult<OutT>> stepFunc, StepOptions options = null)
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

        public Step<NewIT, NewOT> Then<NewIT, NewOT>(Func<NewIT, StepResult<NewOT>> newStepFunc)
            where NewIT : OutT
        {
            var nextStep = new Step<NewIT, NewOT>(newStepFunc);
            return NextStep as Step<NewIT, NewOT>;
        }

        public void RunStep(JobResult jobResult, InT passedObj)
        {
            if (jobResult.HasFailed && !Options.AlwaysRun)
            {
                NextStep?.RunStep(jobResult, default(OutT));
                return;
            }

            StepResult<OutT> stepResult;
            try
            {
                stepResult = StepFunc(passedObj);
            }
            catch (Exception ex)
            {
                stepResult = new StepResult<OutT>()
                {
                    IsSuccess = false,
                    Exception = ex
                };
            }

            if (!stepResult.IsSuccess && Options.StopJobOnError)
            {
                jobResult.HasFailed = true;
            }
            
            NextStep?.RunStep(jobResult, stepResult.PassingObj);
        }
    }

    public class StepOptions
    {
        public bool StopJobOnError = true;
        public bool AlwaysRun = false;
    }
}
