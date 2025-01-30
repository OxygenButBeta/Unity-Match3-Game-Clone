using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3
{
    public abstract class ActiveGameBoard<T> : GameBoardBase
    {
        [SerializeField, TabGroup("Validation Options")]
        protected bool checkNeighbours = true;
        protected Grid<T> grid;
        private IBoardMoveActionValidator<T> _boardMoveActionValidator;
        
        protected abstract void ExecuteValidatedMove(GridElement<T> firstGridElement, GridElement<T> secondGridElement);
        public override void ExecuteMove(BoardActionMove actionMove)
        {
            _boardMoveActionValidator ??= GetValidator();
            if (!_boardMoveActionValidator.ValidateMoveAction(grid, actionMove, out Vector2Int firstElementIndex,
                    out Vector2Int secondElementIndex))
            {
                return;
            }

            GridElement<T> firstGridElement = grid.GetGridElementAt(firstElementIndex);
            GridElement<T> secondGridElement = grid.GetGridElementAt(secondElementIndex);

            if (checkNeighbours && !firstGridElement.IsNeighbourOf(secondGridElement))
                return;

            ExecuteValidatedMove(firstGridElement, secondGridElement);
        }
        protected abstract IBoardMoveActionValidator<T> GetValidator();
    }
    
    public abstract class PassiveGameBoard : GameBoardBase
    {
        protected WorldGrid grid;
    }
}