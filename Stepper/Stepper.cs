using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Stepper
    {
        private List<Step> _steps;

        public Stepper() : this(new List<Step>())
        {
        }

        public Stepper(List<Step> steps)
        {
            _steps = steps;
        }

        public void AddStep(Step step)
        {
            _steps.Add(step);
        }

        public void RemoveStep(Step step)
        {
            _steps.Remove(step);
        }

        public JobResult RunJob()
        {
            var jobResult = new JobResult();
            var stepResults = new List<StepResult>();
            foreach (var step in _steps)
            { 
                var newStepResult = step.RunStep(jobResult);
                stepResults.Add(newStepResult);
                if (!newStepResult.IsSuccess)
                {
                    jobResult.IsSuccess = false;
                }
            }

            return jobResult;
        }
    }
}
