[![Build status](https://ci.appveyor.com/api/projects/status/rn2u831l0xf78ds1?svg=true)](https://ci.appveyor.com/project/Spritely/readmodel-mongo)

# Spritely.ReadModel.Mongo
Provides a default implementation of a Spritely.Cqrs read model database on top of Mongo. Please visit https://github.com/spritely/Cqrs/ for more details on Spritely.Cqrs.

Spritely.ReadModel.Mongo assumes you are developing with Command Query Responsibility Separation (CQRS). When using CQRS another common pattern is Event Sourcing whereby all the commands that have been executed are stored in database log. This log can be replayed to generate a snapshot of the data at any point in time. These snapshots are the respresentation of your model at that moment in time. Best practice is to store these snapshots so that future event source replays can continue from this point forward and avoid having to replay the entire event stream each time. Spritely.ReadModel.Mongo provides most of what you need for creating, reading, updating, and deleting these snapshots in a SQL database. It is not designed to encapsulate the Event Source itself, just the reading and writing of snapshot data.
