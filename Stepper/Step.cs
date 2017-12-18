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

            if (stepResult.EndJob)
            {
                jobResult.HasFailed = !stepResult.IsSuccess;
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

    public class InStep<InT> : Step
    {
        public InStep(Func<InT, StepResult> stepFunc) : base(stepFunc)
        {
        }

        public override void RunStep(JobResult jobResult)
        {
            IStepResult stepResult;
            try
            {
                var previousStepResult = jobResult.StepResults.LastOrDefault() as StepResult<InT>;
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

            if (stepResult.EndJob)
            {
                jobResult.HasFailed = !stepResult.IsSuccess;
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

    public class InOutStep<InT, OutT> : Step
    {
        public InOutStep(Func<InT, StepResult<OutT>> stepFunc) : base(stepFunc)
        {
        }

        public override void RunStep(JobResult jobResult)
        {
            IStepResult stepResult;
            try
            {
                var previousStepResult = jobResult.StepResults.LastOrDefault() as StepResult<InT>;
                if (previousStepResult == null)
                {
                    throw new NullReferenceException("Could not run step. No previous step result.");
                }
                stepResult = StepFunc.DynamicInvoke(previousStepResult.PassingObject) as IStepResult;
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

            jobResult.StepResults.Add(stepResult);

            if (stepResult.EndJob)
            {
                jobResult.HasFailed = !stepResult.IsSuccess;
                return;
            }

            NextStep?.RunStep(jobResult);
        }
       

        // InStep Then
        public InStep<OutT> Then(Func<OutT, StepResult> newStepFunc)
        {
            var nextStep = new InStep<OutT>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }

        // InOutStep Then
        public InOutStep<OutT, NewOutT> Then<NewOutT>(Func<OutT, StepResult<NewOutT>> newStepFunc)
        {
            var nextStep = new InOutStep<OutT, NewOutT>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }
    }

    public class OutStep<OutT> : Step
    {
        public OutStep(Func<StepResult<OutT>> stepFunc) : base(stepFunc)
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
                stepResult = new StepResult<OutT>()
                {
                    IsSuccess = false,
                    EndJob = true,
                    Exception = ex
                };
            }

            jobResult.StepResults.Add(stepResult);

            if (stepResult.EndJob)
            {
                jobResult.HasFailed = !stepResult.IsSuccess;
                return;
            }

            NextStep?.RunStep(jobResult);
        }

        // InStep Then
        public InStep<OutT> Then(Func<OutT, StepResult> newStepFunc)
        {
            var nextStep = new InStep<OutT>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }

        // InOutStep Then
        public InOutStep<OutT, NewOutT> Then<NewOutT>(Func<OutT, StepResult<NewOutT>> newStepFunc)
        {
            var nextStep = new InOutStep<OutT, NewOutT>(newStepFunc);
            Then(nextStep);
            return nextStep;
        }
    }
}