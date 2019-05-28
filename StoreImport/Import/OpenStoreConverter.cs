using System;
using System.Collections.Generic;
using NBrightDNN;
using ZIndex.DNN.OpenStoreImport.Model.Store;

namespace ZIndex.DNN.OpenStoreImport.Import
{
    public class OpenStoreConverter : StoreConverter, IOpenStoreConverter
    {
        private NBrightInfo CreateNBrightInfo(Store store, string typeCode)
        {
            var nbi = new NBrightInfo(true)
            {
                PortalId = 0,//todo: add portalId in store
                GUIDKey = "",
                Lang = store.Culture.ToString(),
                ModifiedDate = DateTime.Now,
                ModuleId = -1,
                ParentItemId = 0,
                RowCount = 0,
                TextData = "",
                TypeCode = typeCode,
                UserId = 0,
                XMLData = "",
               //XMLDoc =  ????
                XrefItemId = 0,

            };
            return nbi;

        }

        public List<NBrightInfo> CreateProductElements(Product product, Store store)
        {
            return new List<NBrightInfo>
                {CreateProduct(product, store), CreateProductLang(product, store), CreateCategoryXRef(product, store)};
        }
        private NBrightInfo CreateProduct(Product product, Store store)
        {
            var nbi = CreateNBrightInfo(store, "PRD");
            nbi.ItemID = product.Id;
            nbi.XMLData = $@"<genxml>
                              <textbox>
                                <txtproductref>{product.Name}</txtproductref>
                                <txtmodelref>{product.Name}</txtmodelref>
                              </textbox>
                              <models>
                                <genxml>
                                  <textbox>
                                    <txtmodelref>{product.Name}</txtmodelref>
                                    <txtunitcost datatype=""double"">{store.ProductUnitCost}</txtunitcost>
                                    <unitqty datatype=""double"">1</unitqty>
                                  </textbox>
                                </genxml>
                              </models>
                              <options />
                              <imgs>
                                <genxml>
                                  <hidden>
                                    <imageurl>{ToImageBaseUrl(product, store.ImageBaseUrl)}</imageurl>
                                    <imagepath>{ToImagePath(product, store)}</imagepath>
                                  </hidden>
                                </genxml>
                              </imgs>
                            </genxml>";
            return nbi;
        }

        private NBrightInfo CreateProductLang(Product product, Store store)
        {
            var nbi = CreateNBrightInfo(store, "PRDLANG");
            nbi.ItemID = product.IdLang;
            nbi.ParentItemId = product.Id;
            nbi.XMLData = $@"<genxml>
                                  <textbox>
                                    <txtproductname update=""lang"">{product.Name}</txtproductname>
                                    <txtmodelname update=""lang"">{product.Name}</txtmodelname>
                                  </textbox>
                                  <models>
                                    <genxml>
                                      <textbox>
                                        <txtmodelname>{product.Name}</txtmodelname>
                                      </textbox>
                                    </genxml>
                                  </models>
                                </genxml>";
            return nbi;
        }

        public List<NBrightInfo> CreateCategoryElements(Category category, Store store)
        {
            return new List<NBrightInfo>
                {CreateCategory(category, store), CreateCategoryLang(category, store)};
        }

        private NBrightInfo CreateCategory(Category category, Store store)
        {
            var nbi = CreateNBrightInfo(store, "CATEGORY");
            nbi.ItemID = category.Id;
            nbi.ParentItemId = category.Parent?.Id ?? 0;
            nbi.XMLData = $@"<genxml>
                                <textbox>
                                    <txtcategoryref>{category.Name}</txtcategoryref>
                                    <propertyref/>
                                </textbox>
                                <dropdownlist>
                                    <ddlgrouptype>cat</ddlgrouptype>
                                    <ddlparentcatid>{category.Parent?.Id ?? 0}</ddlparentcatid>
                                    <ddlattrcode/>
                                    <selectgrouptype>null</selectgrouptype>
                                    <selectcatid/>
                                </dropdownlist>
                             </genxml>";

            return nbi;
        }
    
        public NBrightInfo CreateCategoryLang(Category category, Store store)
        {
            var nbi = CreateNBrightInfo(store, "CATEGORYLANG");
            nbi.ItemID = category.IdLang;
            nbi.ParentItemId = category.Id;
            nbi.XMLData = $@"<genxml>
                            <textbox>
                                <txtcategoryname>{category.Name}</txtcategoryname>
                                <txtcategoryref>{category.Name}</txtcategoryref>
                            </textbox>
                            </genxml>";
            return nbi;
        }
    
        private NBrightInfo CreateCategoryXRef(Product product, Store store)
        {
            var nbi = CreateNBrightInfo(store, "CATXREF");
            nbi.Lang = string.Empty; // force de lang to "", if not the import is not correct (the product is not editable) bug in openstore ?
            nbi.ItemID = product.IdCatXRef;
            nbi.XrefItemId = product.Category.Id;
            nbi.ParentItemId = product.Id;
            nbi.XMLData = $@"<genxml></genxml> ";
            return nbi;
        }
    
    }
}
