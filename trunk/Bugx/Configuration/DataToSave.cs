using System;
using System.Collections.Generic;
using System.Text;

namespace Bugx.Web.Configuration
{
    public enum DataToSave
    {
        None      = 0,
        Session   = 1,
        Cache     = 2,
        Context   = 4,
        Exception = 8,
        All       = Session | Cache | Context | Exception 
    }
}
