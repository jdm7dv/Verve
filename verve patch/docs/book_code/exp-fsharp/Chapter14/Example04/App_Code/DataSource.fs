// ----------------------------
//  Listing 14-7.

#light
namespace FSharpWeb

open System.Web.Configuration
open Microsoft.FSharp.Quotations.Typed
open Microsoft.FSharp.Data.Linq
open nwind

type CategoryInfo = { CategoryID: int;  Name: string; }
type ProductInfo  = { ProductName: string;  Price: System.Decimal; }

module DataSource =
    let db = new Northwind(WebConfigurationManager.ConnectionStrings.
                               Item("NorthwindData").ConnectionString)
    let GetCategories () =
        SQL <@ seq { for c in db.Categories 
                        -> { CategoryID = c.CategoryID
                             Name       = c.CategoryName } } @>

    let GetProducts (categoryId) =
        SQL <@ seq { for p in db.Products
                         when p.CategoryID = categoryId
                         -> { ProductName = p.ProductName
                              Price       = p.UnitPrice.Value } } @>
