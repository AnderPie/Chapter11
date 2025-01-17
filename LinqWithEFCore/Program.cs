AggregateProducts();

ConfigureConsole();
ProductsLookup();
GroupJoinCategoriesAndProducts();
//FilterAndSort();
//JoinCategoriesAndProduct();

/*
 * You might want to create a data structure in-memory that can group items by their category, 
 * and then provide a quick way to look up all products in a category. You can do this with the
 * ILookup<int, Product>? productsByCategoryId = db.Products.ToLookup(keySelector: category => category.CategoryId);
 * 
 * The ToLookup method creates a dictionary-like data structure of key value pairs in memory that has unique 
 * category ids for the key and a collection of products as the value.
 */
