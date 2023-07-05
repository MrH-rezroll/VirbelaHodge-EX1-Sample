using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VirbelaHodge.Scripts.Control;
using VirbelaHodge.Scripts.ScriptableObjects;

public class ScriptableObjectTests
{
    ResourceData resourceData;
    GameplayData gameplayData;

    [OneTimeSetUp]
    public void Setup()
    {
        //Assign
        resourceData = ScriptableObject.CreateInstance<ResourceData>();
        gameplayData = ScriptableObject.CreateInstance<GameplayData>();
    }

    [Test]
    [Category("Resource Data SO Tests")]
    public void ResourceDataDictionariesAreNullBeforeInitialization()
    {
        //Act - Do nothing after creating the ResourceData SO.
        //Assert
        Assert.IsNull(resourceData.MaterialDictionary);
        Assert.IsNull(resourceData.PrefabDictionary);
    }
    [Test]
    [Category("Resource Data SO Tests")]
    public void ResourceDataInitializesMaterialDictionary()
    {
        //Act - Calling the Initialization method should cause the material dictionary to no longer be null.
        resourceData.InitializeDictionaries();
        //Assert
        Assert.IsNotNull(resourceData.MaterialDictionary);
    }
    [Test]
    [Category("Resource Data SO Tests")]
    public void ResourceDataInitializesPrefabDictionary()
    {
        //Act - Calling the Initialization method should cause the prefab dictionary to no longer be null.
        resourceData.InitializeDictionaries();
        //Assert
        Assert.IsNotNull(resourceData.MaterialDictionary);
    }

    [Test]
    [Category("Gameplay Data SO Tests")]
    public void GameplayDataInitializesPlayerPositionTracking()
    {
        //Act
        gameplayData.PlayerPosition = Vector3.zero;
        //Assert
        Assert.AreEqual(gameplayData.PlayerPosition, Vector3.zero);
        //Act
        Vector3 newPos = new(1f, 2f, 3f);
        gameplayData.PlayerPosition = newPos;
        //Assert
        Assert.AreEqual(gameplayData.PlayerPosition, newPos);
    }
}
