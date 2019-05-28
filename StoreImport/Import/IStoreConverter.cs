using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public interface IStoreConverter
    {
        /// <summary>
        /// To the image path.
        /// </summary>
        /// <param name="product">The category.</param>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        string ToImagePath(Product product, Store store);

        /// <summary>
        /// To the image base URL.
        /// </summary>
        /// <param name="product">The category.</param>
        /// <param name="imageBaseUrl">The image base URL.</param>
        /// <returns></returns>
        string ToImageBaseUrl(Product product, string imageBaseUrl);

        string ToCategoryTree(Category category);
    }
}