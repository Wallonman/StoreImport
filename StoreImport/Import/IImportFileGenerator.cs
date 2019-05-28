using System.IO;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public interface IImportFileGenerator
    {
        void Generate(TextWriter writer, Store store);
    }
}