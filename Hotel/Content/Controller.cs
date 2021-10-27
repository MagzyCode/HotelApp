using Hotel.ConnectionSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Content
{
    static class Controller
    {
        public static MyDbContext Context { get; private set; }

        static Controller()
        {
            Context = new MyDbContext();
        }

    }
}
