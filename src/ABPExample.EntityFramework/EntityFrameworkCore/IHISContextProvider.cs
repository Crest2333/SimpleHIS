using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.EntityFramework.EntityFrameworkCore
{
    public interface IHISContextProvider
    {
        IHISCurrentContext GetConext();
    }
}
