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

        //public Step<InT, OutT> AddStep<InT, OutT>(Step<InT, OutT> step)
        //    where InT : class
        //    where OutT : class
        //{
        //    if(FirstStep == default(Step<InT, OutT>))
        //    {
        //        FirstStep = step as IStep<T>;
        //        return FirstStep as Step<InT, OutT>;
        //    }
        //    Step currStep = FirstStep;
        //    while (currStep.NextStep != null)
        //    {
        //        currStep = currStep.NextStep;
        //    }

        //    return currStep.Then(step);

        //}

        public JobResult RunJob()
        {
            var jobResult = new JobResult();
            //var stepResults = new List<StepResult>();
            FirstStep.RunStep(jobResult, FirstInputObject);

            return jobResult;
        }
    }
}
