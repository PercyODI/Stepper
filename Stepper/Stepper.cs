using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Stepper<T>
    {
        private List<Step<T>> _steps;

        public Stepper() : this(new List<Step<T>>())
        {
        }

        public Stepper(List<Step<T>> steps)
        {
            _steps = steps;
        }

        public void AddStep(Step<T> step)
        {
            _steps.Add(step);
        }

        public void RemoveStep(Step<T> step)
        {
            _steps.Remove(step);
        }

        public JobResult RunJob(T initObj)
        {
            var jobResult = new JobResult();
            var stepResults = new List<StepResult>();
            T currObj = initObj;
            foreach(var step in _steps)
            { 
                var newStepResult = step.RunStep(currObj);
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
