using System;
using System.Linq;

namespace Stepper
{
    public class InStep<TIn> : Step
    {
        public InStep(Func<TIn, StepResult> stepFunc) : base(stepFunc)
        {
        }

        public override void RunStep(JobResult jobResult)
        {
            IStepResult stepResult;
            try
            {
                if (!(jobResult.StepResults.LastOrDefault() is StepResult<TIn> previousStepResult))
                {
                    throw new NullReferenceException("Could not run step. No previous step result.");
                }
                stepResult = StepFunc.DynamicInvoke(previousStepResult.PassingObject) as IStepResult;
            }
            catch (Exception ex)
            {
                stepResult = new StepResult()
                {
                    IsSuccess = false,
                    EndJob = true,
                    Exception = ex
                };
            }

            jobResult.StepResults.Add(stepResult);

            if (stepResult == null || stepResult.EndJob)
            {
                jobResult.HasFailed = true;
                return;
            }

            NextStep?.RunStep(jobResult);
        }

        // Regular Step Then
        public RegularStep Then(Func<StepResult> newStepFunc)
        {
            var nextStep = new RegularStep(newStepFunc);
            Then(nextStep);
            return nextStep;
        }

        // OutStep Then
        public OutStep<OutT> Then<OutT>(Func<StepResult<OutT>> newStepFunc)
        {
            var nextStep = new OutStep<OutT>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }
    }
}