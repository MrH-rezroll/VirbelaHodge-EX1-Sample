using UnityEngine;
using VirbelaHodge.Scripts.Contracts;

namespace VirbelaHodge.Scripts.VBHOs.Pawns
{
    /// <summary>
    /// The Pawn class is a base for all assets that would be either player or ai controlled.
    /// It is abstract, because the game requires that a Pawn be of a specified Type.
    /// </summary>
    public abstract class Pawn : MonoBehaviour, IVBHObject
    {
        //The following references ensure that a VBHObject has access to the GameObject Components it will work with.
        public Transform TheTransform { get { return pawnTransform; } set { } }
        public Renderer TheRenderer { get { return pawnRenderer; } set { pawnRenderer = value; } }
        public GameObject TheGameObject { get { return theGameObject; } set { theGameObject = value; } }

        //This is set by a concrete class and used for the business logic to easily identify what this Pawn's purpose is
        //from the base class, without the base class needing to be aware of the child classes specific functionality.
        public VBHORole TheObjectRole { get; set; }

        //The following references allow both the base and concrete(child) class instances to reference their relavent information.
        protected Transform pawnTransform;
        protected Renderer pawnRenderer;
        protected GameObject theGameObject;

        /// <summary>
        /// Initializes neccessary values for a Pawn, and can be overriden from a implementing class.
        /// </summary>
        virtual public void VBHOInitialize()
        {
            pawnTransform = GetComponent<Transform>();
            pawnRenderer = GetComponent<Renderer>();
            theGameObject = gameObject;
        }

        /// <summary>
        /// Empty method required to be stated so that implementing classes can override it.
        /// </summary>
        virtual public void VBHOUpdate(){}
    }
}
