using UnityEngine;

namespace O2.Grid{
    public static class GridSystemUtilities{
        public static bool SimulateGravity<T>(Grid<T> grid, GridGravityOptions<T> gridGravityOptions){
            var hasFallen = false;

            for (var x = 0; x < grid.gridArray.GetLength(0); x++)
            for (var y = 0; y < grid.gridArray.GetLength(1); y++){
                GridNode<T> node = grid.gridArray[x, y];
                if (node.IsStatic || gridGravityOptions.CellFallCondition(node))
                    continue;

                Vector2Int fallToIndex = FindFallDestination(grid, node, gridGravityOptions);
                if (fallToIndex == node.Index)
                    continue;

                hasFallen = true;
                grid.SwapValues(node, grid.GetGridElementAt(fallToIndex));
                gridGravityOptions.onCellFallAction(node);
            }

            return hasFallen;
        }

        private static Vector2Int FindFallDestination<T>(Grid<T> grid, GridNode<T> node,
            GridGravityOptions<T> gridGravityOptions){
            Vector2Int fallToIndex = node.Index;

            while (fallToIndex.y > 0){
                GridNode<T> below = grid.gridArray[node.Index.x, fallToIndex.y - 1];

                if (below.IsFilled){
                    if (!gridGravityOptions.FallDiagonal)
                        break;

                    // bottom right diagonal
                    if (fallToIndex.x + 1 < grid.gridData.width){
                        GridNode<T> rightDiagonal = grid.gridArray[fallToIndex.x + 1, fallToIndex.y - 1];
                        if (!rightDiagonal.IsFilled){
                            fallToIndex = rightDiagonal.Index;
                            continue;
                        }
                    }

                    // bottom left diagonal
                    if (fallToIndex.x - 1 >= 0){
                        GridNode<T> leftDiagonal = grid.gridArray[fallToIndex.x - 1, fallToIndex.y - 1];
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