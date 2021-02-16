# dejacos-crud-app
A dotNet Core based CRUD application w/ Azure CosmosDB

## Hierarchy 0f Entities in Azure Cosmos DB a/c
Depending on the cosmos API used, the aliases may differ

```
DB a/c 
  -> DBs 
    -> Containers(collection/table/graph) 
      -> Items(document/row/node) 
```


When working with document resources, they too have this settable Id property. 
If an Id is not supplied by the user the SDK will automatically generate a new GUID and assign its value to this property before persisting the document in the database.

## Types Of Data Modeling 
- Embedded
- Reference

### ==== self-contained entities / embeded ======
- One-to-few relationships between entities...not 1-to-infinity
- There is embedded data that will grow with limit. *
- There is embedded data that is queried frequently TOGETHER. *

NOT good idea when the embedded data is used often across other items/doc and will change frequently.

### ====== Referencing data ======
Not recommended building systems that would be better suited to a relational database in Azure Cosmos DB, or any other document database, 
but simple relationships are fine and can be useful.
As in, relationships between many container/table enties

When to use...
- Recomeneded for one-to-many relationships.
- Representing many-to-many relationships.
- Referenced data could be unlimited.
- Related/ ref data changes frequently.
- When embeded data changes frequently throughout the day, the only document that needs to be updated is the embed document which will be Referenced.


