using UnityEngine;

namespace O2.Grid{
    public static class GridSystemUtilities{
        public static bool SimulateGravity<T>(Grid<T> grid, GridGravityOptions<T> gridGravityOptions){
            var hasFallen = false;

            for (var x = 0; x < grid.gridArray.GetLength(0); x++)
            for (var y = 0; y < grid.gridArray.GetLength(1); y++){
                GridElement<T> element = grid.gridArray[x, y];
                if (element.IsStatic || gridGravityOptions.CellFallCondition(element))
                    continue;

                Vector2Int fallToIndex = FindFallDestination(grid, element, gridGravityOptions);
                if (fallToIndex == element.Index)
                    continue;

                hasFallen = true;
                grid.SwapValues(element, grid.GetGridElementAt(fallToIndex));
                gridGravityOptions.onCellFallAction(element);
            }

            return hasFallen;
        }

        private static Vector2Int FindFallDestination<T>(Grid<T> grid, GridElement<T> element,
            GridGravityOptions<T> gridGravityOptions){
            Vector2Int fallToIndex = element.Index;

            while (fallToIndex.y > 0){
                GridElement<T> below = grid.gridArray[element.Index.x, fallToIndex.y - 1];

                if (below.IsFilled){
                    if (!gridGravityOptions.FallDiagonal)
                        break; 

                    // bottom right diagonal
                    if (fallToIndex.x + 1 < grid.width){
                        GridElement<T> rightDiagonal = grid.gridArray[fallToIndex.x + 1, fallToIndex.y - 1];
                        if (!rightDiagonal.IsFilled){
                            fallToIndex = rightDiagonal.Index;
                            continue;
                        }
                    }

                    // bottom left diagonal
                    if (fallToIndex.x - 1 >= 0){
                        GridElement<T> leftDiagonal = grid.gridArray[fallToIndex.x - 1, fallToIndex.y - 1];
                        if (!leftDiagonal.IsFilled){
                            fallToIndex = leftDiagonal.Index;
                            continue;
                        }
                    }

                    break; 
                }

                fallToIndex = below.Index;
            }

            return fallToIndex;
        }
    }
}