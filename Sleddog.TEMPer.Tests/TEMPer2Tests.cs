using System;
using System.Reactive.Linq;
using Xunit;

namespace Sleddog.TEMPer.Tests
{
    public class TEMPer2Tests
    {
        [Fact]
        public void Read()
        {
            var temper = new TEMPer2();

            temper.InternalSensor.Subscribe(Console.WriteLine);

            temper.ReadTemperatures();

            Observable.Interval(TimeSpan.FromSeconds(1))
                .TakeWhile(x => x < 5)
                .Wait();
        }
    }
}