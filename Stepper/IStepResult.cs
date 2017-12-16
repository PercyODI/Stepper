using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public interface IStepResult
    {
        bool EndJob { get; set; }
        bool IsSuccess { get; set; }
        Exception Exception { get; set; }
        bool HasPassingObj { get; set; }
    }
}
