using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Stepper<T>
    {
        private Step FirstStep;

        public Stepper()
        {
        }

        public void AddStep(Step step)
        {
            Step currStep = FirstStep;
            while (currStep.NextStep != null)
            {
                currStep = currStep.NextStep;
            }

            currStep.Then(step);
        }

        public JobResult RunJob()
        {
            var jobResult = new JobResult();
            var stepResults = new List<StepResult>();
            FirstStep.RunStep(jobResult);

            return jobResult;
        }
    }
}
