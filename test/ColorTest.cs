using Microsoft.VisualStudio.TestTools.UnitTesting;
using TapoConnect;

namespace Test
{
    [TestClass]
    public class ColorTest
    {
        [TestMethod]
        public void FromHex()
        {
            var color = TapoColor.FromHex("#4287f5");

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromStringHex()
        {
            var color = TapoColor.FromString("#4287f5");

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromHslString()
        {
            var color = TapoColor.FromHsl("hsl(217,90,61)");

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromHslValue()
        {
            var color = TapoColor.FromHsl(217, 90, 61);

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromStringHsl()
        {
            var color = TapoColor.FromString("hsl(217,90,61)");

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromRgbString()
        {
            var color = TapoColor.FromRgb("rgb(66, 135, 245)");

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromRgbValue()
        {
            var color = TapoColor.FromRgb(66, 135, 245);

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }

        [TestMethod]
        public void FromStringRgb()
        {
            var color = TapoColor.FromString("rgb(66, 135, 245)");

            Assert.AreEqual(color.Hue, 217);
            Assert.AreEqual(color.Saturation, 90);
            Assert.AreEqual(color.Brightness, 61);
            Assert.AreEqual(color.ColorTemp, 0);
        }


        [TestMethod]
        public void FromTemperatureString()
        {
            var color = TapoColor.FromTemperature("3800k");

            Assert.IsNull(color.Hue);
            Assert.IsNull(color.Saturation);
            Assert.IsNull(color.Brightness);
            Assert.AreEqual(color.ColorTemp, 3800);
        }

        [TestMethod]
        public void FromTemperatureValue()
        {
            var color = TapoColor.FromTemperature(3800);

            Assert.IsNull(color.Hue);
            Assert.IsNull(color.Saturation);
            Assert.IsNull(color.Brightness);
            Assert.AreEqual(color.ColorTemp, 3800);
        }

        [TestMethod]
        public void FromStringTemperature()
        {
            var color = TapoColor.FromString("3800k");

            Assert.IsNull(color.Hue);
            Assert.IsNull(color.Saturation);
            Assert.IsNull(color.Brightness);
            Assert.AreEqual(color.ColorTemp, 3800);
        }
    }
}