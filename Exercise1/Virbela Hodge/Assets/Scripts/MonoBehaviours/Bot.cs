
namespace VirbelaHodge.Scripts.VBHOs.Pawns
{
    /// <summary>
    /// The concrete implemenation of a Pawn that represents a "Bot".
    /// </summary>
    public class Bot : Pawn
    {
        /// <summary>
        /// Initialize the Pawn base class values and then any specific to a Bot.
        /// </summary>
        public override void VBHOInitialize()
        {
            base.VBHOInitialize();
            TheObjectRole = Contracts.VBHORole.Bot;
        }

        /// <summary>
        /// Run Update loop logic on the Pawn base class, and then any Update logic specific to a Bot.
        /// </summary>
        public override void VBHOUpdate()
        {
            base.VBHOUpdate();
        }
    }
}