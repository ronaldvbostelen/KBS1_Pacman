using System;
using NUnit.Framework;
using WpfGame;
using WpfGame.Controllers.Views;

namespace WpfGame.UnitTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            Assert.Throws<Exception>(() => new GameViewController(new MainWindow(), "dummy"));
        }
    }
}
