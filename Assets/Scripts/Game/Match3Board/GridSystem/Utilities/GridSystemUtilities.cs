namespace O2.Grid{
    public static class GridSystemUtilities{
        public static bool SimulateGravity<T>(Grid<T> grid, GridGravityOptions<T> gridGravityOptions){
            var hasFallen = false;

            foreach (GridElement<T> element in grid.IterateAll(false)){
                var fallToIndex = FindFallDestination(grid, element, gridGravityOptions);

                if (fallToIndex == element.Index.y)
                    continue;

                hasFallen = true;
                grid.SwapValues(element, grid.gridArray[element.Index.x, fallToIndex]);
                gridGravityOptions.onCellFallAction(element);
            }

            return hasFallen;
        }

        private static int FindFallDestination<T>(Grid<T> grid, GridElement<T> element,
            GridGravityOptions<T> gridGravityOptions){
            var fallToIndex = element.Index.y;

            // Check for the lowest index the element can fall to
            while (fallToIndex > 0){
                GridElement<T> above = grid.gridArray[element.Index.x, fallToIndex - 1];

                if (above.IsFilled && !gridGravityOptions.CellFallCondition(above))
                    break; // Stop falling if the cell is filled and condition is not met

                fallToIndex--;
            }

            return fallToIndex;
        }
    }
}