using System;
using System.Reactive.Linq;
using System.Threading;
using Xunit;

namespace Sleddog.TEMPer.Tests
{
    public class TEMPer2Tests
    {
        [Fact]
        public void Read()
        {
            using (var temper = new TEMPer2())
            {
                temper.InternalSensor
                    .Zip(temper.ExternalSensor, (i, e) => new {Internal = i, External = e})
                    .Subscribe(_ =>
                    {
                        Console.WriteLine("Int: {0}", _.Internal);
                        Console.WriteLine("Ext: {0}", _.External);
                    });

                temper.ReadTemperatures();

                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }
    }
}