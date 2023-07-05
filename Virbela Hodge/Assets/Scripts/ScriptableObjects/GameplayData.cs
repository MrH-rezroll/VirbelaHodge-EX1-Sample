using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using VirbelaHodge.Scripts.Contracts;
using VirbelaHodge.Scripts.Control;

namespace VirbelaHodge.Scripts.ScriptableObjects
{
    /// <summary>
    /// This Scriptable Object is a data set representing the state of gameplay asset data that should be tracked
    /// between levels/scenes and/or saved to a file for later runs of the program.
    /// </summary>
    public class GameplayData : ScriptableObject
    {
        public Vector3 PlayerPosition { get; set; }

        /// <summary>
        /// Opens a file stream for writing SaveGameData to a file
        /// </summary>
        public void SaveGameplayDataToFile(int itemCount, int botCount, List<IVBHObject> vBHOs)
        {
            string filePath = Application.persistentDataPath + "/gameplayData.dat";
            FileStream file;

            if (File.Exists(filePath))
            {
                file = File.OpenWrite(filePath);
            }
            else
            {
                file = File.Create(filePath);
            }

            SaveGameData saveGameData = new(PlayerPosition, itemCount, botCount, vBHOs);
            BinaryFormatter binaryFormatter = new();
            binaryFormatter.Serialize(file, saveGameData);
            file.Close();
        }

        /// <summary>
        /// Opens a file stream reading data from a save file and using a BinaryFormatter to construct a SaveGameData object
        /// that populates the GameplayData fields.
        /// </summary>
        /// <returns>bool - true if save data was read, false if not (typically meaning save data didn't exist).</returns>
        public bool LoadGameplayDataFromFile(bool loadPreviousItems, bool loadPreviousBots)
        {
            string filePath = Application.persistentDataPath + "/gameplayData.dat";
            FileStream file;
            if (File.Exists(filePath))
            {
                file = File.OpenRead(filePath);
                BinaryFormatter binaryFormatter = new();
                SaveGameData saveGameData = (SaveGameData)binaryFormatter.Deserialize(file);
                file.Close();
                PlayerPosition = new Vector3(saveGameData.PlayerPositionX, saveGameData.PlayerPositionY, saveGameData.PlayerPositionZ);
                if (loadPreviousItems)
                {
                    int preExistingItemCount = 0;
                    foreach (IVBHObject vBHO in GameControl.Instance.VBHObjects)
                        if (vBHO.TheObjectRole == VBHORole.Item)
                            preExistingItemCount++;
                    if (preExistingItemCount > saveGameData.ItemCount)
                        GameControl.Instance.ItemAmount = preExistingItemCount;
                    else
                        GameControl.Instance.ItemAmount = saveGameData.ItemCount;
                    if (GameControl.Instance.ItemAmount > 0)
                    {
                        GameControl.Instance.GenerateItems();
                        int itemIndex = 0;
                        foreach (IVBHObject vBHO in GameControl.Instance.VBHObjects)
                        {
                            if (vBHO.TheObjectRole == VBHORole.Item)
                            {
                                if (saveGameData.ItemPositionsX != null && itemIndex < saveGameData.ItemPositionsX.Count)
                                {
                                    vBHO.TheTransform.position = new(saveGameData.ItemPositionsX[itemIndex], saveGameData.ItemPositionsY[itemIndex], saveGameData.ItemPositionsZ[itemIndex]);
                                    itemIndex++;
                                }
                            }
                        }
                    }
                }
                if (loadPreviousBots)
                {
                    int preExistingBotCount = 0;
                    foreach (IVBHObject vBHO in GameControl.Instance.VBHObjects)
                        if (vBHO.TheObjectRole == VBHORole.Bot)
                            preExistingBotCount++;
                    if (preExistingBotCount > saveGameData.BotCount)
                        GameControl.Instance.BotAmount = preExistingBotCount;
                    else
                        GameControl.Instance.BotAmount = saveGameData.BotCount;
                    if(GameControl.Instance.BotAmount > 0)
                    {
                        GameControl.Instance.GenerateBots();
                        int botIndex = 0;
                        foreach (IVBHObject vBHO in GameControl.Instance.VBHObjects)
                        {
                            if (vBHO.TheObjectRole == VBHORole.Bot)
                            {
                                if (saveGameData.BotPositionsX != null && botIndex < saveGameData.BotPositionsX.Count)
                                {
                                    vBHO.TheTransform.position = new(saveGameData.BotPositionsX[botIndex], saveGameData.BotPositionsY[botIndex], saveGameData.BotPositionsZ[botIndex]);
                                    botIndex++;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// The subclass used with a BinaryFormatter to save and load data.
        /// </summary>
        [System.Serializable]
        protected class SaveGameData
        {
            public float PlayerPositionX { get; private set; }
            public float PlayerPositionY { get; private set; }
            public float PlayerPositionZ { get; private set; }
            public int ItemCount { get; set; }
            public List<float> ItemPositionsX { get; private set; }
            public List<float> ItemPositionsY { get; private set; }
            public List<float> ItemPositionsZ { get; private set; }
            public int BotCount { get; private set; }
            public List<float> BotPositionsX { get; private set; }
            public List<float> BotPositionsY { get; private set; }
            public List<float> BotPositionsZ { get; private set; }

            public SaveGameData(Vector3 playerPosition, int itemCount, int botCount, List<IVBHObject> vBHOs)
            {
                PlayerPositionX = playerPosition.x;
                PlayerPositionY = playerPosition.y;
                PlayerPositionZ = playerPosition.z;
                ItemCount = itemCount;
                BotCount = botCount;
                ItemPositionsX = new List<float>();
                ItemPositionsY = new List<float>();
                ItemPositionsZ = new List<float>();
                BotPositionsX = new List<float>();
                BotPositionsY = new List<float>();
                BotPositionsZ = new List<float>();
                foreach (IVBHObject vBHO in vBHOs)
                {
                    if (vBHO.TheObjectRole == VBHORole.Item)
                    {
                        ItemPositionsX.Add(vBHO.TheTransform.position.x);
                        ItemPositionsY.Add(vBHO.TheTransform.position.y);
                        ItemPositionsZ.Add(vBHO.TheTransform.position.z);
                    }
                    else if (vBHO.TheObjectRole == VBHORole.Bot)
                    {
                        BotPositionsX.Add(vBHO.TheTransform.position.x);
                        BotPositionsY.Add(vBHO.TheTransform.position.y);
                        BotPositionsZ.Add(vBHO.TheTransform.position.z);
                    }
                }
            }
        }
    }
}