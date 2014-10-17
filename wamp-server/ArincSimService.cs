using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using wamp_server;
using WampSharp.V2;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace wamp_server
{
   public class ArincSimService
   {
       private IWampRealmServiceProvider wampRealmServiceProvider;
       private IArincSimRpcService rpcService;

       // List of topics this sevice can publish to
       private IEnumerable<string> publishTopics = new List<string>() { ARINC_MESSAGE_TOPIC };

       public const String ARINC_MESSAGE_TOPIC = "com.arinc-sim.messages";

       private Dictionary<string, ISubject<String>> topicsDict = new Dictionary<string, ISubject<String>>();

       public ArincSimService(IWampRealmServiceProvider wampRealmServiceProvider)
       {
           this.wampRealmServiceProvider = wampRealmServiceProvider;
           this.rpcService = new ArincSimRpcService();
       }

       // Initialize topic list
       public async void StartAsync()
       {
           foreach(var topic in publishTopics)
               topicsDict[topic] = wampRealmServiceProvider.GetSubject<String>(topic);

           await wampRealmServiceProvider.RegisterCallee(this.rpcService);
       }

       // Publish a string message to the ARINC_MESSAGE_TOPIC topic
       public void PublishArincMessage(string message)
       {
           PublishMessage(ARINC_MESSAGE_TOPIC, message);
       }

       // Publish a string message to some topic, specified by a topic string
       public void PublishMessage(string topic, string message)
       {
           if (topicsDict[topic] == null)
               throw new Exception("Error: Attempted to publish message to nonexistent topic. Try calling StartAsync() first.");
           topicsDict[topic].OnNext(message);
       }
   }
}
