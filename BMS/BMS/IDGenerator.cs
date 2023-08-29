using System;
using System.Collections.Generic;
using System.Text;

namespace BMS
{
    class IDGenerator
    {
        static int id = 0;
        string storeId;
        DateTime date = DateTime.Now;

        public string generate()
        {
            string gid = DateTime.Now.ToString("yyyyMMdd")+"-";
            storeId = gid + ++id;
            
            return storeId;

        }

    }
}

