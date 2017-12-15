﻿using System;
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
        public List<StepResult> StepResults = new List<StepResult>();
    }
}
