using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VirbelaHodge.Scripts.VBHOs.Items;
using VirbelaHodge.Scripts.VBHOs.Pawns;

public class VBHObjectTests
{
    private Item itemComponent;
    private Bot botComponent;
    private Player playerComponent;

    [OneTimeSetUp]
    public void Setup()
    {
        GameObject itemGO = new GameObject();
        GameObject botGO = new GameObject();
        GameObject playerGO = new GameObject();

        //Assign
        itemComponent = itemGO.AddComponent<Item>();
        botComponent = botGO.AddComponent<Bot>();
        playerComponent = playerGO.AddComponent<Player>();
    }

    [Test]
    [Category("VBHObject Initialization")]
    public void ItemTestRoleInitializesToItem()
    {
        // Act
        itemComponent.VBHOInitialize();
        // Assert
        Assert.AreEqual(itemComponent.TheObjectRole, "Item");
    }
    [Test]
    [Category("VBHObject Initialization")]
    public void BotTestRoleInitializesToBot()
    {
        // Act
        botComponent.VBHOInitialize();
        // Assert
        Assert.AreEqual(botComponent.TheObjectRole, "Bot");
    }
    [Test]
    [Category("VBHObject Initialization")]
    public void PlayerTestRoleInitializesToPlayer()
    {
        // Act
        playerComponent.VBHOInitialize();
        // Assert
        Assert.AreEqual(playerComponent.TheObjectRole, "Player");
    }
}
