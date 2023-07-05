using UnityEngine;

namespace VirbelaHodge.Scripts.VBHOs.Pawns
{
    /// <summary>
    /// The concrete implemenation of a Pawn that represents a "Player".
    /// </summary>
    public class Player : Pawn
    {
        //The player's reference to the nearest item/bot. This could be helpful for additional functinoality,
        //but is not used in this demo.
        public GameObject NearestIVBHObject { get; set; }

        /// <summary>
        /// Initialize the Pawn base class values and then any specific to a Player.
        /// </summary>
        public override void VBHOInitialize()
        {
            base.VBHOInitialize();
            TheObjectRole = Contracts.VBHORole.Player;
        }

        /// <summary>
        /// Run Update loop logic on the Pawn base class, and then any Update logic specific to a Player.
        /// </summary>
        public override void VBHOUpdate()
        {
            base.VBHOUpdate();
        }
    }
}
