using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using wamp_server;
using WampSharp.V2;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace MyNamespace
{
    internal class Program
    {
        const string LOCATION = "ws://127.0.0.1:8080/";

        const string REALM = "realm1";

        public static void Main(string[] args)
        {
            using (IWampHost host = new DefaultWampHost(LOCATION))
            {
                IWampHostedRealm realm = host.RealmContainer.GetRealmByName(REALM);

                ArincSimService instance = new ArincSimService(realm.Services);

                instance.StartAsync();

                host.Open();

                PushFakeMessagesAsync(instance);

                Console.WriteLine("Done Initializing");

                Console.ReadLine();
            }
        }

        private static async void PushFakeMessagesAsync(ArincSimService instance)
        {
            int counter = 0;

            IObservable<long> timer =
                Observable.Timer(TimeSpan.FromMilliseconds(0),
                                 TimeSpan.FromMilliseconds(1000));

            IDisposable disposable =
                timer.Subscribe(x =>
                {
                    counter++;

                    Console.WriteLine("Publishing to topic 'com.myapp.topic1': " + counter);
                    try
                    {
                        instance.PublishArincMessage(String.Format("Fake Message Number: {0}",counter));
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex);
                    }
                });

            await timer;
        }

    }
}