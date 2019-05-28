using System.Collections.Generic;
using NBrightDNN;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public interface IOpenStoreConverter : IStoreConverter
    {
        List<NBrightInfo> CreateCategoryElements(Category category, Store store);
        List<NBrightInfo> CreateProductElements(Product product, Store store);
    }
}