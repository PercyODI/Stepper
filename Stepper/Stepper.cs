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

        public Stepper()
        {
        }

        public Step AddStep(Step step)
        {
            if(FirstStep == default(Step))
            {
                FirstStep = step;
                return FirstStep;
            }
            Step currStep = FirstStep;
            while (currStep.NextStep != null)
            {
                currStep = currStep.NextStep;
            }

            return currStep.Then(step);

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
