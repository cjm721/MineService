using MineService_JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Shared.Json
{
    public class Error : Message
    {
        public String errorMessage;

        public Error(String errorMessage)
        {
            this.errorMessage = errorMessage;
        }
    }
}
