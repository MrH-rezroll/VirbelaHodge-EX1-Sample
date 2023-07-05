using System.Collections.Generic;
using UnityEngine;

namespace VirbelaHodge.Scripts.Contracts
{
    // Enum used for base class to make make decisions about an instance, based on its role in the system.
    // Set by concrete implementing classes. Originally, just a string this became an enum for improved efficiency,
    // and less chance of human error when specifying the role in a concrete implementation.
    public enum VBHORole { Item, Bot, Player }

    /// <summary>
    /// This Interface is a Contract that forces all implementing classes to be "Virbela Hodge Objects" (vBHObjects, vBHOs).
    /// All objects implementing this interface are usable within the business logic of this example.
    /// </summary>
    public interface IVBHObject
    {

        // Required fields
        public Transform TheTransform { get; set; }
        public Renderer TheRenderer { get; set; }
        public GameObject TheGameObject { get; set; }
        public VBHORole TheObjectRole { get; set; }

        // Required public methods
        public void VBHOInitialize();
        public void VBHOUpdate() { /*Default Implementation*/ } //This method is defaulted because not all vBHOs will need to use it.

        /// <summary>
        /// Method to organize VBHObjects in the scene hirearchy. It works by creating a nested empty game object structure with
        /// usefull, descriptive names. It then iterates over the supplied list and puts the objects in their appropriate places.
        /// </summary>
        /// <param name="vBHObjects">The list of VBHObjects to be organized.</param>
        static void OrganizeVBHObjects(List<IVBHObject> vBHObjects)
        {
            GameObject vBHORoot = new() { name = "VBHOs_Root" };
            vBHORoot.transform.position = Vector3.zero;
            GameObject itemsRoot = new() { name = "Items_Root" };
            itemsRoot.transform.position = Vector3.zero;
            itemsRoot.transform.SetParent(vBHORoot.transform);
            GameObject botsRoot = new () { name = "Bots_Root" };
            botsRoot.transform.position = Vector3.zero;
            botsRoot.transform.SetParent(vBHORoot.transform);
            foreach (IVBHObject iVBHO in vBHObjects)
            {
                if (iVBHO.TheObjectRole == VBHORole.Item)
                    iVBHO.TheTransform.SetParent(itemsRoot.transform);
                else if (iVBHO.TheObjectRole == VBHORole.Bot)
                    iVBHO.TheTransform.SetParent(botsRoot.transform);
                else
                    iVBHO.TheTransform.SetParent(vBHORoot.transform);
            }
        }

        /// <summary>
        /// This function conducts the specific logic of creating one new VBHO and adding it to the list of VBHO's.
        /// </summary>
        /// <param name="vBHObjects">The list of VBHOs that we want to add one to.</param>
        /// <param name="prefabToGenerate">The prefab of a VBHO that we want to add</param>
        /// <param name="maxAmount">The maximum amount of this type of VBHO in the scene, this is used to determine the
        ///                         random range max value for placing VBHOs randomly.</param>
        /// <param name="rndSeed">The seed value for this type of VBHO</param>
        /// <returns>IVBHObject - The VBHO that has been created, randomly placed and put in the list of VBHOs</returns>
        static IVBHObject AddOneIVBHObject(List<IVBHObject> vBHObjects, GameObject prefabToGenerate, int maxAmount, int rndSeed = 1)
        {
            if (maxAmount < 5) // Ensure we have enough of a random range to pull unique values and not got stuck in a while loop.
                maxAmount = 5;
            Random.InitState(rndSeed);
            List<Vector3> vBHOLocations = new List<Vector3>();
            foreach (IVBHObject iVBHO in vBHObjects) vBHOLocations.Add(iVBHO.TheTransform.position);
            Vector3 randomLocation = new Vector3(Random.Range(1, maxAmount), 0, Random.Range(1, maxAmount));
            while (vBHOLocations.Contains(randomLocation)) // ensure we do not get an existing location.
                randomLocation = new Vector3(Random.Range(1, maxAmount), 0, Random.Range(1, maxAmount));
            vBHOLocations.Add(randomLocation);
            IVBHObject thisItem = Object.Instantiate(prefabToGenerate).GetComponent<MonoBehaviour>() as IVBHObject;
            thisItem.VBHOInitialize();
            thisItem.TheTransform.position = randomLocation;
            vBHObjects.Add(thisItem);
            return thisItem;
        }

        /// <summary>
        /// This method will remove a VBHO from a supplied list of VBHOs.
        /// </summary>
        /// <param name="vBHObjects">The List of VBHOs to remove an item from.</param>
        /// <param name="vBHORoleToRemove">The role of the item to remove.</param>
        /// <returns>IVBHObject - A reference to the VBHO that was removed from the list, to be used by the function
        ///          that wanted it removed. (usually to Destroy it)</returns>
        static IVBHObject RemoveOneIVBHObject(List<IVBHObject> vBHObjects, VBHORole vBHORoleToRemove)
        {
            IVBHObject thisVBHO = null;
            vBHObjects.Reverse(); //Reverse the list so that the most recently added come up first, shortening the search time.
            foreach (IVBHObject iVBHO in vBHObjects)
                if (iVBHO.TheObjectRole == vBHORoleToRemove)
                {
                    thisVBHO = iVBHO;
                    break;
                }
            vBHObjects.Reverse(); //Put the list back in original order after removing the most recently added VBHO of the specified role.
            return thisVBHO;
        }

        /// <summary>
        /// Generates and randomly places a specified number of VBHObjects of a specified type. This generator accounts for any
        /// VBHOs manually placed within the scene and will only generate the difference between the number of VBHOs of the specified
        /// type that exist, and the max amount that can exist.
        /// </summary>
        /// <param name="vBHObjects">The list of VBHObjects that will be added to.</param>
        /// <param name="prefabToGenerate">The prefab of the type of VBHO to generate.</param>
        /// <param name="maxAmount">The total amount of the specified type that should exist.</param>
        /// <param name="rndSeed">The random seed value for this type of VBHO.</param>
        static void GenerateIVBHObjects(List<IVBHObject> vBHObjects, GameObject prefabToGenerate, int maxAmount, int rndSeed = 1)
        {
            Random.InitState(rndSeed);
            int preExistingPrefabToGenerateCount = 0;
            List<Vector3> vBHOLocations = new();
            foreach (IVBHObject iVBHO in vBHObjects)
            {
                vBHOLocations.Add(iVBHO.TheTransform.position);
                if (prefabToGenerate.GetComponent<IVBHObject>().GetType() == iVBHO.GetType())
                    preExistingPrefabToGenerateCount++;
            }
            for (int i = 0; i < maxAmount - preExistingPrefabToGenerateCount; i++)
            {
                Vector3 randomLocation = new(Random.Range(1, maxAmount), 0, Random.Range(1, maxAmount));
                while (vBHOLocations.Contains(randomLocation)) // ensure we don't get a same random location.
                    randomLocation = new Vector3(Random.Range(1, maxAmount), 0, Random.Range(1, maxAmount));
                vBHOLocations.Add(randomLocation);
                IVBHObject thisItem = Object.Instantiate(prefabToGenerate).GetComponent<MonoBehaviour>() as IVBHObject;
                thisItem.VBHOInitialize();
                thisItem.TheTransform.position = randomLocation;
                vBHObjects.Add(thisItem);
            }
        }
    }
}