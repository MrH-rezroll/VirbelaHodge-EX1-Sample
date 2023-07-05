using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using VirbelaHodge.Scripts.ScriptableObjects;
using VirbelaHodge.Scripts.VBHOs.Pawns;
using VirbelaHodge.Scripts.Contracts;
using VirbelaHodge.Scripts.VBHOs.Items;
using Unity.Collections;

namespace VirbelaHodge.Scripts.Control
{
    /// <summary>
    /// The GameControl class is the business logic for this example. It controls the behavior of the Main scene's assets.
    /// It makes decisions by monitoring user input and environment conditions such as proximity of player to other assets.
    /// </summary>
    public class GameControl : MonoBehaviour
    {
        public static GameControl Instance { get; private set; } //The single Instance making GameControl a Singlton.

        public int ItemAmount { get { return itemAmount; } set { itemAmount = value; } }
        public int BotAmount { get { return botAmount; } set { botAmount = value; } }
        public List<IVBHObject> VBHObjects { get { return vBHObjects; } private set { vBHObjects = value; } }

        [TextArea, ReadOnly, SerializeField]
        private string BotAndItemPlacementNote = "While playing, press Q and A to add/remove Bots. Press W and S to add/remove Items.";
        [Space]
        [TextArea, ReadOnly, SerializeField]
        private string BotAndItemGenerationNote = "Regardless of how many items or bots are present when saving, " +
            "there will always be at least the items or bots specifically placed in the scene before playing. " +
            "If you would like the bot/item count to be completely accurate on load, put no items/bots in the scene " +
            "and place them while playing.";

        /// <summary>
        /// The section in the editor allowing designers to easily identify and adjust Item values relavent to their tasks.
        /// </summary>
        [Space]
        [Header("Item Settings")]
        [Space]
        [SerializeField]
        private Color baseItemColor;
        [SerializeField]
        private Color highlightedItemColor;
        [Tooltip("Loading previous Items will ignore the Item Amount")]
        [SerializeField]
        private bool loadPreviousItems = true;
        [SerializeField]
        private int itemAmount = 5;
        [SerializeField]
        private int itemGeneratorSeed = 1;

        /// <summary>
        /// The section in the editor allowing designers to easily identify and adjust Bot values relavent to their tasks.
        /// </summary>
        [Space]
        [Header("Bot Settings")]
        [Space]
        [SerializeField]
        private Color baseBotColor;
        [SerializeField]
        private Color highlightedBotColor;
        [SerializeField]
        [Tooltip("Loading previous Bots will ignore the Bot Amount")]
        private bool loadPreviousBots = true;
        [SerializeField]
        private int botAmount = 5;
        [SerializeField]
        private int botGeneratorSeed = 1;

        private List<IVBHObject> vBHObjects;
        private Player player;
        private GameplayData gameplayData;
        private ResourceData resourceData;
        private int gameState;
        private bool canManipulateVBHObjectCount;

        #region Unity Hooks

        /// <summary>
        /// Unity's OnValidate Hook is used to allow Edit Time changes to item/bot colors.
        /// Using this hook allows the desired fucntionality to effect only the materals used whereas the [ExecuteInEditMode]
        /// tag would cause all functionality to work in Edit Time, creating mass chaos.
        /// </summary>
        private void OnValidate()
        {
            resourceData = ScriptableObject.CreateInstance<ResourceData>();
            resourceData.InitializeDictionaries();
            resourceData.MaterialDictionary["BaseItem"].color = baseItemColor;
            resourceData.MaterialDictionary["HighlightedItem"].color = highlightedItemColor;
            resourceData.MaterialDictionary["BaseBot"].color = baseBotColor;
            resourceData.MaterialDictionary["HighlightedBot"].color = highlightedBotColor;
        }

        /// <summary>
        /// Enusure everything that needs to be setup for first initialization is ready to go.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this); // Ensure that only one Instance of GameControl exists, forcing Instance to be a Singleton.
            else Instance = this;

            gameplayData = ScriptableObject.CreateInstance<GameplayData>();
            resourceData = ScriptableObject.CreateInstance<ResourceData>();
            resourceData.InitializeDictionaries();

            vBHObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IVBHObject>().ToList();
            foreach (IVBHObject vBHO in vBHObjects) vBHO.VBHOInitialize();
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        /// <summary>
        /// Initialize values for all elements of the game, any elements requiring pre initialization steps should occure in Awake.
        /// </summary>
        private void Start()
        {
            if (gameplayData.LoadGameplayDataFromFile(loadPreviousItems, loadPreviousBots))
                player.TheTransform.position = gameplayData.PlayerPosition;
            if(!loadPreviousItems)
                GenerateItems();
            if(!loadPreviousBots)
                GenerateBots();
            IVBHObject.OrganizeVBHObjects(vBHObjects);
            SetGameState(1);
        }

        /// <summary>
        /// Unity's Update hook is used to select the appropriate logic for the game's current state.
        /// Different state's represent different phases of the program. For example, state 0 could be the splash screen
        /// and main menu. State 1 is where gameplay happens. Additional states could be for Game Over conditions, pause
        /// conditions, or any other situation where the core gameplay shouldn't run. For this demo, only the gameplay
        /// state is used.
        /// </summary>
        private void Update()
        {
            switch (gameState)
            {
                case 0: break;
                case 1:
                    gameplayData.PlayerPosition = player.TheTransform.position; //Ensure GameplayData reflects the player's current position for saving.
                    if (canManipulateVBHObjectCount) {
                        if (Input.GetKeyDown("w")) AddOneItem();
                        else if (Input.GetKeyDown("s")) RemoveLastItem();
                        else if (Input.GetKeyDown("q")) AddOneBot();
                        else if (Input.GetKeyDown("a")) RemoveLastBot();
                    }
                    if (vBHObjects != null)
                        foreach (IVBHObject iVBHO in vBHObjects) iVBHO.VBHOUpdate();
                    gameplayData.PlayerPosition = player.TheTransform.position;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Used to start saving Player/Item/Bot amount and locations before the application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            gameplayData.SaveGameplayDataToFile(itemAmount, botAmount, vBHObjects);
        }

        #endregion

        #region Public Functions
        /// <summary>
        /// Calls IVBHObject to generate Items. This is public so that the load system can access it when restoring Item amount
        /// and locations.
        /// </summary>
        public void GenerateItems()
        {
            IVBHObject.GenerateIVBHObjects(vBHObjects, resourceData.PrefabDictionary["Item"], itemAmount, itemGeneratorSeed);
        }

        /// <summary>
        /// Calls IVBHObject to generate Bots. This is public so that the load system can access it when restoring Bot amount
        /// and locations.
        /// </summary>
        public void GenerateBots()
        {
            IVBHObject.GenerateIVBHObjects(vBHObjects, resourceData.PrefabDictionary["Bot"], botAmount, botGeneratorSeed);
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Set's the games new state and initializes any setup required before Update runs that state's logic.
        /// </summary>
        /// <param name="newState">The state the game will enter.</param>
        private void SetGameState(int newState)
        {
            gameState = newState;
            switch (gameState)
            {
                case 0: break;
                case 1:
                    canManipulateVBHObjectCount = true;
                    StartCoroutine(NearestVBHObject());
                    break;
                default: break;
            }
        }

        /// <summary>
        /// This will add a single new Item adhering to the IVBHObject contract and place it approriatley in the scene.
        /// </summary>
        private void AddOneItem()
        {
            canManipulateVBHObjectCount = false;
            itemAmount++;
            IVBHObject thisVBHO = IVBHObject.AddOneIVBHObject(vBHObjects, resourceData.PrefabDictionary["Item"], itemAmount, itemGeneratorSeed);
            thisVBHO.TheTransform.SetParent(GameObject.Find("Items_Root").transform);
            StartCoroutine(CleanUpUnusedVBHOs());
        }

        /// <summary>
        /// This will add a single new Bot adhering to the IVBHObject contract and place it approriatley in the scene.
        /// </summary>
        private void AddOneBot()
        {
            canManipulateVBHObjectCount = false;
            botAmount++;
            IVBHObject thisVBHO = IVBHObject.AddOneIVBHObject(vBHObjects, resourceData.PrefabDictionary["Bot"], botAmount, botGeneratorSeed);
            thisVBHO.TheTransform.SetParent(GameObject.Find("Bots_Root").transform);
            StartCoroutine(CleanUpUnusedVBHOs());
        }

        /// <summary>
        /// This will remove the last Item adhering to the IVBHObject contract from the scene, and all appropriate places in code.
        /// </summary>
        private void RemoveLastItem()
        {
            itemAmount--;
            canManipulateVBHObjectCount = false;
            IVBHObject vBHOToDestroy = IVBHObject.RemoveOneIVBHObject(vBHObjects, VBHORole.Item);
            if (vBHOToDestroy != null) Destroy(vBHOToDestroy.TheGameObject);
            // Final cleanup is done in a couroutine because the Destroy call won't actually execute until the end of the frame.
            // We need to delay cleanup in the vBHObjects list until we know the Object has been destroyed.
            StartCoroutine(CleanUpUnusedVBHOs());
        }

        /// <summary>
        /// This will remove the last Bot adhering to the IVBHObject contract from the scene, and all appropriate places in code.
        /// </summary>
        private void RemoveLastBot()
        {
            botAmount--;
            canManipulateVBHObjectCount = false;
            IVBHObject vBHOToDestroy = IVBHObject.RemoveOneIVBHObject(vBHObjects, VBHORole.Bot);
            if (vBHOToDestroy != null) Destroy(vBHOToDestroy.TheGameObject);
            // Final cleanup is done in a couroutine because the Destroy call won't actually execute until the end of the frame.
            // We need to delay cleanup in the vBHObjects list until we know the Object has been destroyed.
            StartCoroutine(CleanUpUnusedVBHOs());
        }

        #endregion

        #region Private Coroutines

        /// <summary>
        /// Removes any null VBHObjects from the vBHObject List and then resumes the nearest item/bot search after cleanup.
        /// </summary>
        /// <returns>WaitForEndOfFrame - The signal that the frame has ended, allowing code execution of the coroutine to resume.</returns>
        private IEnumerator CleanUpUnusedVBHOs()
        {
            yield return new WaitForEndOfFrame();
            vBHObjects.RemoveAll(vBHO => !vBHO.TheGameObject);
            canManipulateVBHObjectCount = true;
            StartCoroutine(NearestVBHObject());
        }

        /// <summary>
        /// Iterates over the VBHObjects list and returns a reference the the VBHO closest to the Player.
        /// This is using a bubble sort that could be optimized by replacing with a KD-Tree given more time.
        /// As part of the sorting task, this function is also making decisions about which shared material the VBHObjects
        /// should have. This is done here to reduce the amount of times a foreach loop is required.
        /// Additionally, this search is done in a coroutine to keep the main thread free to process core gameplay functionality.
        /// It should help with performance. The "yield return null" is placed such that the each iteration of the while loop
        /// will wait for a new frame to execute. This means the loop runs slower, but it's not noticable to the user and evens
        /// performance in other areas.
        /// </summary>
        /// <returns>null - The signal to wait until the next frame to continue the while loop.</returns>
        private IEnumerator NearestVBHObject()
        {
            while (gameState == 1 && canManipulateVBHObjectCount)
            {
                try
                {
                    IVBHObject nearestIVBHO = vBHObjects[0];
                    if (nearestIVBHO.TheGameObject.GetComponent<Player>()) { nearestIVBHO = vBHObjects[1]; }

                    foreach (IVBHObject iVBHO in vBHObjects)
                    {
                        if (!iVBHO.TheGameObject.GetComponent<Player>())
                        {
                            if (iVBHO.TheGameObject.GetComponent<Item>())
                                iVBHO.TheRenderer.sharedMaterial = resourceData.MaterialDictionary["BaseItem"];
                            else if (iVBHO.TheGameObject.GetComponent<Bot>())
                                iVBHO.TheRenderer.sharedMaterial = resourceData.MaterialDictionary["BaseBot"];

                            if (Vector3.Distance(player.TheTransform.position, nearestIVBHO.TheTransform.position) > Vector3.Distance(player.TheTransform.position, iVBHO.TheTransform.position))
                                nearestIVBHO = iVBHO;
                        }
                    }

                    if (nearestIVBHO.TheGameObject.GetComponent<Item>())
                        nearestIVBHO.TheRenderer.sharedMaterial = resourceData.MaterialDictionary["HighlightedItem"];
                    else if (nearestIVBHO.TheGameObject.GetComponent<Bot>())
                        nearestIVBHO.TheRenderer.sharedMaterial = resourceData.MaterialDictionary["HighlightedBot"];

                    player.NearestIVBHObject = nearestIVBHO.TheGameObject;
                }
                catch
                {
                    //This Warning basically means an out of bounds error occured, but puts that information into more
                    //"designer friendly" language that would help the designer understand what is needed from them.
                    Debug.LogWarning("Searching for a nearest item or bot failed. Do you have at least one item or bot in the scene?");
                }
                yield return null;
            }
        }
        #endregion
    }
}