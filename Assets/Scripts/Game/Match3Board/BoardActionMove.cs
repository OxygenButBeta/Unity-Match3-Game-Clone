using UnityEngine;

namespace Match3
{
    /// <summary>
    /// A Struct to represent a move on the board.
    /// It contains the start position of the move and the direction of the move.
    /// </summary>
    public struct BoardActionMove
    {
        /// <summary>
        /// Start position of the move in screen space.
        /// </summary>
        public Vector3 startPositionScreenToWorld;
        /// <summary>
        /// Direction of the move on the board. 
        /// </summary>
        public Vector2Int DesignatedDirection;
    }
}