using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.ExceptionСlasses
{
    class IsCorrectChooseException : Exception
    {
        public IsCorrectChooseException (string message) : base(message)
        { }
    }
}
