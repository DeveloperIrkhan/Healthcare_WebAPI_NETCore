using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Exceptions
{
    internal class ConflictException:Exception

    {
        public ConflictException(string message) : base(message)
        {
            
        }
    }
}
