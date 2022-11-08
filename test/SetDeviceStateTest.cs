using Microsoft.VisualStudio.TestTools.UnitTesting;
using TapoConnect;

namespace Test
{
    [TestClass]
    public class SetDeviceStateTest
    {
        [TestMethod]
        public void EqualityComparison()
        {
            var hex = new TapoSetBulbState(TapoColor.FromHex("34eba4"), true);
            var rgb = new TapoSetBulbState(TapoColor.FromRgb("52, 235, 164"), true);

            Assert.AreEqual(hex, rgb);
        }

        [TestMethod]
        public void NotEqualityComparison()
        {
            var hex = new TapoSetBulbState(TapoColor.FromHex("eb34ae"), true);
            var rgb = new TapoSetBulbState(TapoColor.FromRgb("52, 235, 164"), true);

            Assert.AreNotEqual(hex, rgb);
        }
    }
}