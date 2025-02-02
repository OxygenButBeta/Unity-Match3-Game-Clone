using System;
using O2.Grid;
using UnityEngine;

namespace Match3
{
    /// <summary>
    /// This interface is used to create validators for the moves on the board.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBoardMoveActionValidator
    {
        /// <summary>
        /// Validates the move on the board. If the move is valid, it returns true and the indices of the elements that will be moved.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="swipeActionData"> </param>
        /// <param name="firstElementIndex"></param>
        /// <param name="secondElementIndex"></param>
        /// <returns></returns>
        public bool ValidateMoveAction(WorldGrid grid, SwipeActionData swipeActionData);

    }
}