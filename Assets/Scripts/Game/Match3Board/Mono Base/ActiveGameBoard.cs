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
        // The grid that represents the game board.
        public Grid<T> Grid{ get; private set; }
        public override WorldGrid WorldGrid => Grid;

        [SerializeField, TabGroup("Validation Options")] protected bool checkNeighbours = true;

        /// <summary>
        /// Validators to validate the move action.
        /// </summary>
        [SerializeReference] ISwipeActionValidator[] _boardMoveActionValidators;

        /// <summary>
        /// This method is called when a move is validated by the base game board class and validated by the validator.
        /// </summary>
        /// <param name="firstGridNode"></param>
        /// <param name="secondGridNode"></param>
        protected abstract void ExecuteValidatedMove(GridNode<T> firstGridNode, GridNode<T> secondGridNode);


        public override void ExecuteMove(SwipeActionData swipeActionData){
            if (_boardMoveActionValidators != null)
                if (_boardMoveActionValidators.Any(validator => !validator.ValidateSwapAction(Grid, swipeActionData)))
                    return;

            GridNode<T> firstGridNode =
                Grid.GetGridElementWithWorldPosition(swipeActionData.startPositionScreenToWorld);

            GridNode<T> secondGridNode =
                Grid.GetGridElementAt(firstGridNode.Index + swipeActionData.DesignatedDirection);


            if (!secondGridNode.IsFilled)
                return;

            if (checkNeighbours && !firstGridNode.IsNeighbourOf(secondGridNode))
                return;

            ExecuteValidatedMove(firstGridNode, secondGridNode);
        }

        /// <summary>
        /// This method will be called after the grid is created.
        /// </summary>
        protected virtual void OnAwake(){
        }

        private void Awake(){
            Grid = new Grid<T>(gridData);
            OnAwake();
        }
    }
}