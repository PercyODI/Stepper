using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class Step<T>
    {
        private StepOptions Options { get; set; }
        private Func<T, StepResult> StepFunc { get; set; }
        public Step(Func<T, StepResult> stepFunc, StepOptions options = null)
        {
            if (options == null)
                options = new StepOptions();

            Options = options;
            StepFunc = stepFunc;
        }

        internal StepResult RunStep(T obj)
        {
            StepResult stepResult;
            try
            {
                stepResult = StepFunc(obj);
            }
            catch (Exception ex)
            {
                stepResult = new StepResult()
                {
                    IsSuccess = false,
                    Exception = ex
                };
            }

            return stepResult;
        }
    }

    public class StepOptions
    {
        public bool StopJobOnError = true;
        public bool AlwaysRun = false;
    }
}
