using System;
using System.IO;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public class StoreConverter : IStoreConverter
    {
        /// <summary>
        /// To the image path.
        /// </summary>
        /// <param name="product">The category.</param>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// category
        /// or
        /// imageBasePath
        /// </exception>
        public string ToImagePath(Product product, Store store)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (store == null) throw new ArgumentNullException(nameof(store));

//            var relativePath = category.FullPath.Remove(0, store.StoreRootPath.Length);
//            return Path.Combine(store.ImageBasePath, store.StoreName, relativePath, category.ImageFilename);

            return Path.Combine(store.ImageBasePath, product.ImageFilename);
        }

        /// <summary>
        /// To the image base URL.
        /// </summary>
        /// <param name="product">The category.</param>
        /// <param name="imageBaseUrl">The image base URL.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// category
        /// or
        /// imageBaseUrl
        /// </exception>
        public string ToImageBaseUrl(Product product, string imageBaseUrl)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (imageBaseUrl == null) throw new ArgumentNullException(nameof(imageBaseUrl));

            var uri = new Uri(string.Concat(imageBaseUrl.TrimEnd('/', '\\'), "/", product.ImageFilename) , UriKind.Absolute);

            return uri.ToString();
        }

        public string ToCategoryTree(Category category)
        {
            if (category.Parent == null)
                return category.Name;

            return ToCategoryTree(category.Parent) + " > " + category.Name;
        }
    }
}