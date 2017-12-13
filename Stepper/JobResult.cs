using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepper
{
    public class JobResult : IResult
    {
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
    }
}
