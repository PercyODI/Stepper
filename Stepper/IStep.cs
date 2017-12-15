using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public interface IStep<in T> where T : class
    {
        void RunStep(JobResult jobResult, T passedObj = null);
    }
}
