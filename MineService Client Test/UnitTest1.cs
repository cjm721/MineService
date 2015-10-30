using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPFMineServiceTest;
using System.Windows.Controls;

namespace MineService_Client_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLoadingNewServerTabXaml()
        {
            TabItem item = MainWindow.GetNewServerTabItem();
            Assert.Equals("Monster", item.Name);
        }
    }
}
