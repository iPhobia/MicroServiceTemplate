﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public interface IAction
    {
        ActionResult Execute(int queueId);
    }
}
