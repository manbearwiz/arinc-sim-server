using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace wamp_server
{

    public interface IArincSimRpcService
    {
        [WampProcedure("com.arinc-sim.ping")]
        void Ping();

        [WampProcedure("com.arinc-sim.add2")]
        int Add2(int a, int b);

        [WampProcedure("com.arinc-sim.sayhello")]
        string SayHello(string name = "foo");
    }

    class ArincSimRpcService : IArincSimRpcService
    {
        public void Ping()
        {
        }

        public int Add2(int a, int b)
        {
            Console.WriteLine("Entering Add2");
            int result = a + b;
            Console.WriteLine("Leaving Add2 with value={0}", result);
            return result;
        }

        public string SayHello(string name = "foo")
        {
            return String.Format("Hello {0}", name);
        }
    }
}
