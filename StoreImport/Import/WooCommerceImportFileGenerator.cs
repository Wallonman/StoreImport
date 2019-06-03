using System;
using System.IO;
using System.Linq;
using System.Text;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public class WooCommerceImportFileGenerator : IImportFileGenerator
    {
        private readonly IStoreConverter _converter;

        public WooCommerceImportFileGenerator(IStoreConverter converter)
        {
            _converter = converter;
        }
        public void Generate(TextWriter writer, Store store)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (!store.Products.Any()) throw new ArgumentNullException(nameof(store.Products));
            if (!store.Categories.Any()) throw new ArgumentNullException(nameof(store.Categories));
            if (store.Culture == null) throw new ArgumentNullException("store.Culture");
            if (store.ImageBasePath == null) throw new ArgumentNullException("store.ImageBasePath");
            if (store.ImageBaseUrl == null) throw new ArgumentNullException("store.ImageBaseUrl");

            var strXml = new StringBuilder();
            strXml.AppendLine(@"type;name;Published;regular_price;category_ids;images");

            store.Products.ForEach(product =>
            {
                var categoryTree = _converter.ToCategoryTree(product.Category);
                var str = $"simple;{product.Name};1;{store.ProductUnitCost};{categoryTree};{product.ImageFilename}";
                strXml.AppendLine(str);
            });


            writer.Write(strXml.ToString());

        }

        public string FileExtension => "csv";
    }
}