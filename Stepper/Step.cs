using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stepper.Exceptions;

namespace Stepper
{
    public abstract class Step
    {
        protected Delegate StepFunc { get; set; }
        protected Step NextStep { get; set; }

        protected Step()
        {
        }

        public Step(Delegate stepFunc)
        {
            StepFunc = stepFunc;
        }

        public abstract void RunStep(JobResult jobResult);

        public Step Then(Step nextStep)
        {
            NextStep = nextStep;
            return NextStep;
        }


    }

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
                var previousStepResult = jobResult.StepResults.LastOrDefault() as StepResult<TIn>;
                if (previousStepResult == null)
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