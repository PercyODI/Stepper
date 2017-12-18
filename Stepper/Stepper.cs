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

        public InStep<InT> AddFirstStep<InT>(Func<InT, StepResult> stepFunc)
        {
            var newStep = new InStep<InT>(stepFunc);
            FirstStep = newStep;
            return newStep;
        }

        public InOutStep<InT, OutT> AddFirstStep<InT, OutT>(Func<InT, StepResult<OutT>> stepFunc)
        {
            var newStep = new InOutStep<InT, OutT>(stepFunc);
            FirstStep = newStep;
            return newStep;
        }

        public OutStep<OutT> AddFirstStep<OutT>(Func<StepResult<OutT>> stepFunc)
        {
            var newStep = new OutStep<OutT>(stepFunc);
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