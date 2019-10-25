using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka_client
{
	class Program
	{
		public static void Main(string[] args)
		{
			print("main");
			doThing();
		}

		async static void doThing()
		{
			Producer producer = new Producer();
			//Producer.useProduce(); // either
			//producer.mthd().Wait(); // will work
		}

		static void print(string msg)
		{
			Console.WriteLine(msg);
		}
	}
}
