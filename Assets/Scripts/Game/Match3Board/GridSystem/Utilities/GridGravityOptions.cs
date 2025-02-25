using System;

namespace O2.Grid{
    public class GridGravityOptions<T>{
        /// <summary>
        /// This predicate will be used to determine if a cell should fall or not.
        /// </summary>
        public readonly Predicate<GridNode<T>> CellFallCondition;

        /// <summary>
        /// This action will be called when a cell falls.
        /// </summary>
        public readonly Action<GridNode<T>> onCellFallAction;

        public readonly bool FallDiagonal;

        public GridGravityOptions(Predicate<GridNode<T>> cellFallCondition, Action<GridNode<T>> onCellFallAction,
            bool fallDiagonal){
            CellFallCondition = cellFallCondition;
            this.onCellFallAction = onCellFallAction;
            FallDiagonal = fallDiagonal;
        }
    }
}