using System.Linq;
using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3{
    /// <summary>
    /// Base class for all game boards.
    /// It contains the logic to execute a move on the board.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ActiveGameBoard<T> : GameBoardBase{
        [SerializeField, TabGroup("Validation Options")]
        protected bool checkNeighbours = true;

        // The grid that represents the game board.
        public Grid<T> grid;

        /// <summary>
        /// Validators to validate the move action.
        /// </summary>
        [SerializeReference] IBoardMoveActionValidator[] _boardMoveActionValidators;

        /// <summary>
        /// This method is called when a move is validated by the base game board class and validated by the validator.
        /// </summary>
        /// <param name="firstGridElement"></param>
        /// <param name="secondGridElement"></param>
        protected abstract void ExecuteValidatedMove(GridElement<T> firstGridElement, GridElement<T> secondGridElement);

        public override void ExecuteMove(BoardSwipeActionData swipeActionData){
            if (_boardMoveActionValidators != null)
                if (_boardMoveActionValidators.Any(validator => !validator.ValidateMoveAction(grid, swipeActionData)))
                    return;

            GridElement<T> firstGridElement =
                grid.GetGridElementWithWorldPosition(swipeActionData.startPositionScreenToWorld);

            GridElement<T> secondGridElement =
                grid.GetGridElementAt(firstGridElement.Index + swipeActionData.DesignatedDirection);


            if (!secondGridElement.IsFilled){
                return;
            }

            if (checkNeighbours && !firstGridElement.IsNeighbourOf(secondGridElement))
                return;

            ExecuteValidatedMove(firstGridElement, secondGridElement);
        }
    }
}