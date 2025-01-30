using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3
{
    /// <summary>
    /// Base class for game boards.
    /// Created to use serialization and polymorphism.
    /// </summary>
    public abstract class GameBoardBase : MonoBehaviour
    {
        [SerializeField, TabGroup("Grid Options"), Tooltip("Width of the grid")]
        protected int width;

        [SerializeField, TabGroup("Grid Options"), Tooltip("Height of the grid")]
        protected int height;

        [SerializeField, TabGroup("Grid Options"), Tooltip("Size of the cell")]
        protected float cellSize;

        [SerializeField, TabGroup("Grid Options"), Tooltip("Origin of the grid")]
        protected Vector2 origin;

        /// <summary>
        /// The method that will be called when a move is executed.
        /// </summary>
        /// <param name="boardActionMove"></param>
        public virtual void ExecuteMove(BoardActionMove boardActionMove)
        {
            // This method will be overridden by the child classes.
        }
    }
}