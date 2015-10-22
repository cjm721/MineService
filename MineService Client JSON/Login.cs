using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineService_Client_JSON
{
    public class Login
    {
        public String Username;
        public String Password;

        public Login(String Username, String Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
