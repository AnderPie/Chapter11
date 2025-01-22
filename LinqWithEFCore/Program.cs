ProcessSettings();
OutPutProductsAsXML();
PagingProducts();
ConfigureConsole();
AggregateProducts();
ProductsLookup();
GroupJoinCategoriesAndProducts();
//FilterAndSort();
//JoinCategoriesAndProduct();

#region ToLookup(): Creating in-memory structure from lookups
/*
 * You might want to create a data structure in-memory that can group items by their category, 
 * and then provide a quick way to look up all products in a category. You can do this with the
 * ILookup<int, Product>? productsByCategoryId = db.Products.ToLookup(keySelector: category => category.CategoryId);
 * 
 * The ToLookup method creates a dictionary-like data structure of key value pairs in memory that has unique 
 * category ids for the key and a collection of products as the value.
 */
#endregion
#region Checking for an empty sequence
/*
 * LINQ Count() method. Usually a bad choice because it must enumerate through the whole sequence
 * LINQ Any() method. Better than count but not as good as next two options
 * Length property of a sequence if the sequence has one.  Every [] has one,
 * Count property of the sequence f it has one. Any ICollection or ICollection<T> will have Count as part of its contract
 */
#endregion