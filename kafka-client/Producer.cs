using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace kafka_client
{
	class Producer
	{
		//public static async Task Main(string[] args)
		public static async Task produce(string message)
		{
			Console.WriteLine("produce ");
			var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

			// If serializers are not specified, default serializers from
			// `Confluent.Kafka.Serializers` will be automatically used where
			// available. Note: by default strings are encoded as UTF8.
			using (var p = new ProducerBuilder<Null, string>(config).Build())
			{
				Console.WriteLine("using");
				try
				{
					Console.WriteLine("try");
					var dr = await p.ProduceAsync("testTopicName", new Message<Null, string> { Value = message }).ConfigureAwait(false);
					Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
				}
				catch (ProduceException<Null, string> e)
				{
					Console.WriteLine("catch");
					Console.WriteLine($"Delivery failed: {e.Error.Reason}");
				}
				Console.WriteLine("did try");
			}
		}

		public Func<Task> mthd = async () =>
		{
			Console.WriteLine("mthd");
			using (var producer = new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
			{
				Console.WriteLine("using2");
				var singlePartitionTopic = "testTopicName";
				var dr = await producer.ProduceAsync(
					singlePartitionTopic,
					new Message<Null, string> { Value = "test string" });
				Debug.Assert(0 == producer.Flush(TimeSpan.FromSeconds(10)));
				Debug.Assert(Offset.Unset != dr.Offset);
			}
		};

		public static void useProduce()
		{
			Console.WriteLine("useProduce");
			var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };

			Action<DeliveryReport<Null, string>> handler = r =>
				Console.WriteLine(!r.Error.IsError
					? $"Delivered message to {r.TopicPartitionOffset}"
					: $"Delivery Error: {r.Error.Reason}");

			using (var p = new ProducerBuilder<Null, string>(conf).Build())
			{
				for (int i = 0; i < 100; ++i)
				{
					Console.WriteLine(i);
					p.Produce("testTopicName", new Message<Null, string> { Value = i.ToString() }, handler);
				}

				// wait for up to 10 seconds for any inflight messages to be delivered.
				p.Flush(TimeSpan.FromSeconds(10));
			}
		}

		public async void testProduce()
		{
			string testMsg = "Testing produce()";
			Console.WriteLine(testMsg);
			await produce(testMsg);
		}

		public static void p1()
		{
			Console.WriteLine("print Producer p1");
		}

		public  void p2()
		{
			Console.WriteLine("print Producer p2");
		}
	}
}