using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.Hadoop.Avro;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IoTHubAvro
{
    [DataContract(Namespace = "Avro")]
    [KnownType(typeof(string))]
    public class TestDTO
    {
        [DataMember]
        public int TestInt { get; set; }

        [DataMember]
        public string TestProp;
    }

    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "AvroHub.azure-devices.net";
        static string deviceKey = "WR3w0+ZEbh+8gwrRfXo0YKV7MZKHsJAuMs/4Yg2AFZw=";
        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("TestDeviceAvro", deviceKey), TransportType.Mqtt);
            int x = 0;
            while (true)
            {
                SendDeviceToCloudMessagesAsync(x++);
                System.Threading.Thread.Sleep(1000);
            }
        }

        private static async void SendDeviceToCloudMessagesAsync(int x)
        {
            var memoryStreamWriter = new MemoryStream();

            var avroSerializerSettings = new AvroSerializerSettings();
            avroSerializerSettings.Resolver = new AvroDataContractResolver(allowNullable: true);

            var root = new
            {
                car = new
                {
                    name = "Ford",
                    owner = "Henry"
                }
            };
            string json = JsonConvert.SerializeObject(root);

            TestDTO Obj = new TestDTO { TestInt = x, TestProp = json };

            using (var w = AvroContainer.CreateWriter<TestDTO>(memoryStreamWriter, Codec.Deflate))
            {
                using (var sequentialWriter = new SequentialWriter<TestDTO>(w, 24))
                {
                    sequentialWriter.Write((TestDTO)Obj);
                }
            }

            var memAr = memoryStreamWriter.ToArray();
            Message message = new Message(memAr);

            await deviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, x);

            await Task.Delay(1000);
        }
    }
}
