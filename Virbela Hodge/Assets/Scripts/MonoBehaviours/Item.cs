using UnityEngine;
using VirbelaHodge.Scripts.Contracts;

namespace VirbelaHodge.Scripts.VBHOs.Items
{
    /// <summary>
    /// An Item is a non player or ai controlled VBHObject that is interactable. It does not specifiy a
    /// VBHOUpdate method because it is defaulted in the contract and an Item does not (yet) make use of it.
    /// </summary>
    public class Item : MonoBehaviour, IVBHObject
    {
        public Transform TheTransform { get; set; }
        public Renderer TheRenderer { get; set; }
        public GameObject TheGameObject { get; set; }
        public VBHORole TheObjectRole { get; set; }

        /// <summary>
        /// Initialize required values for an Item.
        /// </summary>
        public void VBHOInitialize()
        {
            TheTransform = GetComponent<Transform>();
            TheRenderer = GetComponent<Renderer>();
            TheGameObject = gameObject;
            TheObjectRole = VBHORole.Item;
        }
    }
}