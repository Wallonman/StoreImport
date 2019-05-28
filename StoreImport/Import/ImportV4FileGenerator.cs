using System;
using System.IO;
using System.Linq;
using System.Text;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public class ImportV4FileGenerator : IImportFileGenerator
    {
        private readonly IOpenStoreConverter _openStoreConverter;

        public ImportV4FileGenerator(IOpenStoreConverter openStoreConverter)
        {
            _openStoreConverter = openStoreConverter;
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

            // initialize the id for models and images (use a value > product or category id)
            var id = store.Products.Max(product => product.Id) + store.Categories.Max(category => category.Id);

            var strXml = new StringBuilder("<root>");

            // add products
            store.Products.ForEach(product => 
                _openStoreConverter.CreateProductElements(product, store)
                    .ForEach(nbi => strXml.Append(nbi.ToXmlItem())));

            // add categories
            store.Categories.ForEach(category =>
                _openStoreConverter.CreateCategoryElements(category, store)
                    .ForEach(nbi => strXml.Append(nbi.ToXmlItem())));

            strXml.Append("</root>");

            writer.Write(strXml.ToString());

        }

    }
}