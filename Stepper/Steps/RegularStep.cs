using System;

namespace Stepper
{
    public class RegularStep : Step
    {
        public RegularStep(Func<StepResult> stepFunc) : base(stepFunc)
        {
        }

        public override void RunStep(JobResult jobResult)
        {
            IStepResult stepResult;
            try
            {
                stepResult = StepFunc.DynamicInvoke() as IStepResult;
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
        public OutStep<TOut> Then<TOut>(Func<StepResult<TOut>> newStepFunc)
        {
            var nextStep = new OutStep<TOut>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }
    }
}