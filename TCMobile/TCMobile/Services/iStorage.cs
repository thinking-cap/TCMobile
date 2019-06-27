using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TCMobile.Services
{
    public interface iStorage
    {       
        Task<UInt64> GetFreeSpace();
    }
}
