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

            temper.InternalSensor
                .Zip(temper.ExternalSensor, (i, e) => new {Internal = i, External = e})
                .Subscribe(_ =>
                {
                    Console.WriteLine("Int: {0}", _.Internal);
                    Console.WriteLine("Ext: {0}", _.External);
                });

            temper.ReadTemperatures();

            Observable.Interval(TimeSpan.FromSeconds(1))
                .TakeWhile(x => x < 5)
                .Wait();
        }
    }
}