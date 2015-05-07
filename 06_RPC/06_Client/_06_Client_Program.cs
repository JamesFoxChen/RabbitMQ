using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//The client code is slightly more involved:

//We establish a connection and channel and declare an exclusive 'callback' queue for replies.
//We subscribe to the 'callback' queue, so that we can receive RPC responses.
//Our call method makes the actual RPC request.
//Here, we first generate a unique correlationId number and save it - the while loop will use this value to catch the appropriate response.
//Next, we publish the request message, with two properties: replyTo and correlationId.
//At this point we can sit back and wait until the proper response arrives.
//The while loop is doing a very simple job, for every response message it checks if the correlationId is the one we're looking for. If so, it saves the response.
//Finally we return the response back to the user.

namespace _06_Client 
{
    class _06_Client_Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RPCClient();

            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30"); //计算结果
            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();

            Console.ReadKey();
        }
    }
}
