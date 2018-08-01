# StorageEmulatorTracer
A simple tracer for Storage Emulator alowing for analyzing queries. While the exact syntax of queries against Table Storage may differ, it enables you to quickly diagnose whether you are following some basic principles when working with Azure Storage Tables.

## Checked principles
Currently StorageEmulatorTracer checks whether:
* you are querying records by providing a partition key
* you are querying records by providing a row key
* you are querying records by providing TOP N 

More principles(like using query projection) will be added soon.

## How to run it?
Simply download and build a project. A working packaghe will be released sooner or later.

## How does it look like?
![GitHub Logo](https://image.ibb.co/jav5Ae/how_it_works.png)

## How does it work?
StorageEmulatorTracer leverages the way how Storage Explorer works - as it internally uses SQL Server to materialize queries, it is possible to use SQL Server Profiler to get each query data and analyze it.

## Limitations
Current implementation of StorageEmulatorTracer has some limitations:
* it has to be run as 32-bit application
* it supports SQL Server 2016 only(though one can easily fix that by rebuilding it with proper dependency version)
* it has hardcoded tracing profile so some data may not be available

## Questions?
Feel free to post an issue in case of any questions or comments.
