using System;

namespace Stepper
{
    public class OutStep<TOut> : Step
    {
        public OutStep(Func<StepResult<TOut>> stepFunc) : base(stepFunc)
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
                stepResult = new StepResult<TOut>()
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

        // InStep Then
        public InStep<TOut> Then(Func<TOut, StepResult> newStepFunc)
        {
            var nextStep = new InStep<TOut>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }

        // InOutStep Then
        public InOutStep<TOut, TNewOut> Then<TNewOut>(Func<TOut, StepResult<TNewOut>> newStepFunc)
        {
            var nextStep = new InOutStep<TOut, TNewOut>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }
    }
}