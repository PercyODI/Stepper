using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public interface IStep
    {
        void RunStep(JobResult jobResult);
        void RunStep(JobResult jobResult, object passedObj);
    }
}
