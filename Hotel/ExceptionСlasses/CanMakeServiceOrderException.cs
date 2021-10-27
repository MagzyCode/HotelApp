using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hotel.ExceptionСlasses
{
    class CanMakeServiceOrderException : Exception
    {
        public CanMakeServiceOrderException(string message) : base(message)
        { }
    }
}
