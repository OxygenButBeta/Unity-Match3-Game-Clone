using System.Diagnostics.CodeAnalysis;
using O2.Grid;
using UnityEngine;

namespace Match3{
    /// <summary>
    /// Base class for game boards.
    /// Created to use serialization and polymorphism.
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
    public abstract class GameBoardBase : MonoBehaviour{
        public abstract WorldGrid WorldGrid { get; }
        [field: SerializeField] public GridData gridData{ get; private set; }

        public Vector2 GetWorldCenter() => new Vector3(gridData.width / 2, gridData.height / 2) * gridData.cellSize +
                                           gridData.origin;

        /// <summary>
        /// The method that will be called when a move is executed.
        /// </summary>
        /// <param name="swipeActionData"></param>
        public virtual void ExecuteMove(SwipeActionData swipeActionData){
            // This method will be overridden by the child classes.
        }

    }
}