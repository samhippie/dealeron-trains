using NUnit.Framework;

namespace dealeron_trains.tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void ParsesString_WithoutExceptions()
        {
            Assert.DoesNotThrow(() => Program.ReadGraphFromString("AB3, NK9, RI9, KD9, KL9"));
        }
    }
}
