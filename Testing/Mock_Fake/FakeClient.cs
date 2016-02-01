using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Mock_Fake
{
    public class FakeClient : MineService_Server.Client
    {
        public String message;

        public FakeClient(Object a, Object b) : base(null,null)
        {

        }

        public FakeClient() : base(null,null)
        {

        }

        public override void sendMessage(String message) {
            this.message = message;
        }
    }
}
