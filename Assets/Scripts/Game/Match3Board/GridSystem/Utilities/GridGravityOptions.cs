using System;

namespace O2.Grid{
    public class GridGravityOptions<T>{
        /// <summary>
        /// This predicate will be used to determine if a cell should fall or not.
        /// </summary>
        public readonly Predicate<GridElement<T>> CellFallCondition;
        
        /// <summary>
        /// This action will be called when a cell falls.
        /// </summary>
        public readonly Action<GridElement<T>> onCellFallAction;

        public GridGravityOptions( Predicate<GridElement<T>> cellFallCondition, Action<GridElement<T>> onCellFallAction){
            this.CellFallCondition = cellFallCondition;
            this.onCellFallAction = onCellFallAction;
        }
    }
}