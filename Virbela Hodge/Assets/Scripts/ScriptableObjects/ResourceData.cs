using System.Collections.Generic;
using UnityEngine;

namespace VirbelaHodge.Scripts.ScriptableObjects
{
    /// <summary>
    /// This Scriptable Object contains data that is a library of Dictionaries for various Resources/Prefabs
    /// that will be used throughout the example.
    /// </summary>
    public class ResourceData : ScriptableObject
    {
        public Dictionary<string, Material> MaterialDictionary { get; private set; }
        public Dictionary<string, GameObject> PrefabDictionary { get; private set; }
        public Color BaseItemColor {
            get { return MaterialDictionary["BaseItem"].color; }
            set {
                MaterialDictionary["BaseItem"].color = value;
            }
        }
        public Color HighlightedItemColor
        {
            get { return MaterialDictionary["HighlightedItem"].color; }
            set
            {
                MaterialDictionary["HighlightedItem"].color = value;
            }
        }
        public Color BaseBotColor
        {
            get { return MaterialDictionary["BaseBot"].color; }
            set
            {
                MaterialDictionary["BaseBot"].color = value;
            }
        }
        public Color HighlightedBotColor
        {
            get { return MaterialDictionary["HighlightedBot"].color; }
            set
            {
                MaterialDictionary["HighlightedBot"].color = value;
            }
        }

        /// <summary>
        /// Populates the Dictionary data.
        /// </summary>
        public void InitializeDictionaries()
        {
            MaterialDictionary = new Dictionary<string, Material>
            {
                { "BaseItem", Resources.Load<Material>("Materials/BaseItem") },
                { "HighlightedItem", Resources.Load<Material>("Materials/HighlightedItem") },
                { "BaseBot", Resources.Load<Material>("Materials/BaseBot") },
                { "HighlightedBot", Resources.Load<Material>("Materials/HighlightedBot") },
                { "Player", Resources.Load<Material>("Materials/Player") }
            };
            PrefabDictionary = new Dictionary<string, GameObject>
            {
                { "Item", Resources.Load<GameObject>("Prefabs/Item") },
                { "Bot", Resources.Load<GameObject>("Prefabs/Bot") }
            };
        }
    }
}