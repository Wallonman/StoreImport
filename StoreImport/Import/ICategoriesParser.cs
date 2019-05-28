using System.Collections.Generic;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public interface ICategoriesParser
    {

        /// <summary>
        /// Parses recursively the categories from the root path
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        List<Category> Parse(string rootPath);
    }
}