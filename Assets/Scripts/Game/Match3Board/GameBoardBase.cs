using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3{
    /// <summary>
    /// Base class for game boards.
    /// Created to use serialization and polymorphism.
    /// </summary>
    public abstract class GameBoardBase : MonoBehaviour{
        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Width of the grid")]
        protected int width{ get; private set; }

        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Height of the grid")]
        protected int height{ get; private set; }

        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Size of the cell")]
        protected float cellSize{ get; private set; }

        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Origin of the grid")]
        protected Vector2 origin{ get; private set; }

        /// <summary>
        /// The method that will be called when a move is executed.
        /// </summary>
        /// <param name="swipeActionData"></param>
        public virtual void ExecuteMove(SwipeActionData swipeActionData){
            // This method will be overridden by the child classes.
        }
    }
}