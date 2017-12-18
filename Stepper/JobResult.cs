using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class JobResult
    {
        public bool HasFailed = false;
        public Exception Exception { get; set; }
        public string FailedMessage { get; set; }
        public List<IStepResult> StepResults { get; set; }
        //public List<StepResult<T>> StepResults = new List<StepResult<object>>();

        public JobResult()
        {
            StepResults = new List<IStepResult>();
        }
    }
}
