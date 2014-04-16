unity3d-prefab-pool
===================

Simple Prefab Pool for Unity3D

**Summary**: This project provides a simple implementation for pool of prefabs to avoid performance issues with runtime prefab instantiation.

The project contains an example of usage. You can also read about simple prefab pool from [my blog post on blogger](http://android-by-example.blogspot.com/2013/09/simple-pool-of-prefabs-for-unity3d-game.html). 

***

The usage is extremely simple. First you create an instance of PrefabPool (there are overloaded constructors for flexibility). The constructor with all parameters:
### Creating the pool
```csharp
	// Create pool of cubes containing instantiated cubePrefabs
	PrefabPool cubesPool = 
		new PrefabPool(cubePrefab, parent, initialPoolSize, growth, maxPoolSize);
```

This is the code that creates a pool of `cubePrefab` transform objects, instantiates `initialPoolSize` number of them (items are inactive after instantiation). If there is a need to instantiate more prefabs they will be batch instantiated `growth` number at a time. You can also limit the size of your pool by specifying `maxPoolSize`.  

Now the only methods you should care about are `ObtainPrefab` and `RecyclePrefab`. When you obtain prefab from the pool it is set to be active and you specify its new position, rotation and scale. 
### Obtaining instantiated prefab from the pool

```csharp
Transform retrievedCubeInstance1 = cubesPool.ObtainPrefabInstance(newParent.gameObject); 
```
### Recycling active instantiated prefab
```csharp
cubesPool.RecyclePrefab(retrievedCubeInstance1);
```
