using O2.Grid;
using UnityEngine;

namespace Match3{
    /// <summary>
    /// This class is used to validate the moves on the match3 board.
    /// It checks if the move is valid and returns the indices of the elements that will be moved.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PositionToIndexValidator : IBoardMoveActionValidator{
        public bool ValidateMoveAction(WorldGrid grid, SwipeActionData swipeActionData){
            
            // Check if the move is valid
            if (swipeActionData.DesignatedDirection == Vector2Int.zero)
                return false;

            // Check if there is a cell at the start position
            if (!grid.TryToGetElementIndexFromWorldPosition(swipeActionData.startPositionScreenToWorld,
                    out var firstElementIndex)){
                Debug.Log("There is no cell at the start position");
                return false;
            }

            // Check if there is a cell at the end position
            if (!grid.IsIndexWithinBounds(firstElementIndex + swipeActionData.DesignatedDirection)){
                Debug.Log("There is no cell at the end position");
                return false;
            }

            return true;
        }
    }
}