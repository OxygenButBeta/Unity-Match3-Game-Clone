using O2.Grid;
using UnityEngine;

namespace Match3
{
    /// <summary>
    /// This class is used to validate the moves on the match3 board.
    /// It checks if the move is valid and returns the indices of the elements that will be moved.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Match3MoveActionValidator : IBoardMoveActionValidator<Candy>
    {
        public bool ValidateMoveAction(Grid<Candy> grid, BoardActionMove boardActionMove, out Vector2Int firstElementIndex,
            out Vector2Int secondElementIndex)
        {
            firstElementIndex = secondElementIndex = Vector2Int.zero;

            // Check if the move is valid
            if (boardActionMove.DesignatedDirection == Vector2Int.zero)
                return false;

            // Check if there is a cell at the start position
            if (!grid.GetElementAtWorldPosition(boardActionMove.startPositionScreenToWorld, out firstElementIndex))
            {
                Debug.Log("There is no cell at the start position");
                return false;
            }

            // Check if there is a cell at the end position
            if (!grid.IsIndexWithinBounds(firstElementIndex + boardActionMove.DesignatedDirection))
            {
                Debug.Log("There is no cell at the end position");
                return false;
            }

            secondElementIndex = firstElementIndex + boardActionMove.DesignatedDirection;

            return true;
        }
    }
}