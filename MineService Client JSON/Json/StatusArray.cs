using MineService_JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Shared.Json
{
    public class StatusArray : Message
    {
        public Status[] array;

        public StatusArray(Status[] array)
        {
            this.array = array;
        }
    }
}
