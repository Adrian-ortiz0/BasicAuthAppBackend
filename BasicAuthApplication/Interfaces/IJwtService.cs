using BasicAuthDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthApplication.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
