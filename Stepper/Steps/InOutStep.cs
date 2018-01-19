using System;
using System.Linq;

namespace Stepper
{
    public class InOutStep<TIn, TOut> : Step
    {
        public InOutStep(Func<TIn, StepResult<TOut>> stepFunc) : base(stepFunc)
        {
        }

        public override void RunStep(JobResult jobResult)
        {
            IStepResult stepResult;
            try
            {
                var previousStepResult = jobResult.StepResults.LastOrDefault() as StepResult<TIn>;
                if (previousStepResult == null)
                {
                    throw new NullReferenceException("Could not run step. No previous step result.");
                }
                stepResult = StepFunc.DynamicInvoke(previousStepResult.PassingObject) as IStepResult;
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