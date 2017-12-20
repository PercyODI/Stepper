using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Stepper
    {
        private Step FirstStep;
        private object FirstInputObject;
        private Action ActionOnSuccess { get; set; }
        private Action<string, Exception> ActionOnFailure { get; set; }

        public RegularStep AddFirstStep(Func<StepResult> stepFunc)
        {
            var newStep = new RegularStep(stepFunc);
            FirstStep = newStep;
            return newStep;
        }

        public InStep<TIn> AddFirstStep<TIn>(Func<TIn, StepResult> stepFunc)
        {
            var newStep = new InStep<TIn>(stepFunc);
            FirstStep = newStep;
            return newStep;
        }

        public InOutStep<TIn, TOut> AddFirstStep<TIn, TOut>(Func<TIn, StepResult<TOut>> stepFunc)
        {
            var newStep = new InOutStep<TIn, TOut>(stepFunc);
            FirstStep = newStep;
            return newStep;
        }

        public OutStep<TOut> AddFirstStep<TOut>(Func<StepResult<TOut>> stepFunc)
        {
            var newStep = new OutStep<TOut>(stepFunc);
            FirstStep = newStep;
            return newStep;
        }

        public Stepper AddActionOnSuccess(Action onSuccessAction)
        {
            ActionOnSuccess = onSuccessAction;
            return this;
        }

        public Stepper AddActionOnFailure(Action<string, Exception> onFailureAction)
        {
            ActionOnFailure = onFailureAction;
            return this;
        }

        public JobResult RunJob()
        {
            var jobResult = new JobResult();
            FirstStep.RunStep(jobResult);

            if (jobResult.HasFailed)
                ActionOnFailure?.Invoke(jobResult.FailedMessage, jobResult.Exception);
            else
                ActionOnSuccess?.Invoke();

            return jobResult;
        }

        public JobResult RunJob(object firstInputObject)
        {
            var jobResult = new JobResult();

            var firstStepType = FirstStep.GetType();
            if (firstStepType == typeof(InStep<>) || firstStepType == typeof(InOutStep<,>))
            {
                jobResult.StepResults.Add(new StepResult<object>()
                {
                    EndJob = false,
                    PassingObject = firstInputObject
                });
            }
            FirstStep.RunStep(jobResult);

            if (jobResult.HasFailed)
                ActionOnFailure?.Invoke(jobResult.FailedMessage, jobResult.Exception);
            else
                ActionOnSuccess?.Invoke();

            return jobResult;
        }
    }
}