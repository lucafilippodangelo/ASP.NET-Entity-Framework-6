# ASP.NET Entity Framework 6

Playing around with Entity Framework 6 and Learning of new features. The code is well commented.

Main Areas:
- InsertNinja();
- InsertMultipleNinjas(); //LD "AddRange" can insert a list of items at the same time
- SimpleNinjaQueries();
- QueryAndUpdateNinja();
- DeleteNinja();
- RetrieveDataWithFind();
- RetrieveDataWithStoredProc();
- DeleteNinjaWithKeyValue();
- DeleteNinjaViaStoredProcedure();
- QueryAndUpdateNinjaDisconnected();
- InsertNinjaWithEquipment();
- SimpleNinjaGraphQuery();
- ProjectionQuery();
- QueryAndUpdateNinjaDisconnected();
- ReseedDatabase();

Some notes:
- DbContext: useful to define which domain objects(the list of the different classes) we will work.
  - by setting "NinjaContext", is possible to map the database and also tracking the changes, but "**DbContext**" still don't know nothing about our classes.
- the "**DbSet**" has the responsability to maintain in memory the collection of the objects of the classes -> we execute the query by "DbSet" -> we added the riferiment to the project with the classes "using System.Data.Entity;"
