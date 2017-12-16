using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Stepper<T>
    {
        private IStep<T> FirstStep;
        private T FirstInputObject;
        private Action ActionOnSuccess { get; set; }
        private Action<string, Exception> ActionOnFailure { get; set; }

        public Stepper()
        {
        }

        public void SetFirstInputObject(T obj)
        {
            FirstInputObject = obj;
        }

        public Step<InT, OutT> AddFirstStep<InT, OutT>(Step<InT, OutT> step)
        {
            FirstStep = step as IStep<T>;
            return FirstStep as Step<InT, OutT>;
        }

        public Step<InT, OutT> AddFirstStep<InT, OutT>(Func<InT, StepResult<OutT>> StepFunc)
            where InT : T
        {
            var newStep = new Step<InT,OutT>(StepFunc);
            FirstStep = newStep as IStep<T>;
            return newStep;
        }

        public Stepper<T> AddActionOnSuccess(Action onSuccessAction)
        {
            ActionOnSuccess = onSuccessAction;
            return this;
        }

        public Stepper<T> AddActionOnFailure(Action<string, Exception> onFailureAction)
        {
            ActionOnFailure = onFailureAction;
            return this;
        }

        public bool RunJob()
        {
            var jobResult = new JobResult();
            //var stepResults = new List<StepResult>();
            FirstStep.RunStep(jobResult, FirstInputObject);
            if (jobResult.HasFailed)
                ActionOnFailure?.Invoke(jobResult.FailedMessage, jobResult.Exception);
            else
                ActionOnSuccess?.Invoke();

            return !jobResult.HasFailed;
        }
    }
}
