using System;
using System.Collections.Generic;
using Aardvark.Base;
using LasLaz;
using NUnit.Framework;

namespace LasLazTester
{
	public class Tests
	{

		[Test]
		public void TestForHandler()
		{
			var handler = new Handler();
			var data = handler.RunWithFile();
			Assert.IsNotEmpty(data, "data should contain some shit");

			foreach (var d in data)
				Console.WriteLine(d);

			Assert.Pass();
		}

		[Test]
		public void TestForReading()
		{
			var handler = new Handler();
			var cloud = handler.ReadFile();
			Assert.IsNotNull(cloud);
			
			var stack = new HashSet<Byte>();
			
			foreach (var c in cloud.classifications)			
				stack.Add(c);
			
			foreach (var c in stack)
				Console.WriteLine(c);
		}
	}
}