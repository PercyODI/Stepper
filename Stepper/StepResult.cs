using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class StepResult : IResult
    {
        public bool EndJob { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public object PassingObj { get; set; }
    }
}
