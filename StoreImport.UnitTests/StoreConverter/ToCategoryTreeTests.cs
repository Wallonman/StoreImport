using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Moq;
using NUnit.Framework;
using ZIndex.DNN.OpenStoreImport.Import;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.UnitTests.FolderParser
{
    [TestFixture]
    public class ToCategoryTreeTests : TestBase
    {
        private Category _category;
        private StoreConverter _converter;
        private string _actualTree;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _category = new Category{
                Name = "child",
                Parent = new Category{
                    Name = "parent", Parent = new Category{Name = "grand parent"}}};
            _converter = new StoreConverter();

            _actualTree = _converter.ToCategoryTree(_category);
        }

        [TearDown]
        public override void TestTearDown()
        {
            base.TestTearDown();
        }


        [Test]
        public void TreeIsValid()
        {
            Assert.AreEqual("grand parent > parent > child", _actualTree);
        }


    }
}