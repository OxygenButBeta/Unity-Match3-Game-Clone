using UnityEngine;

namespace O2.Grid{
    /// <summary>
    /// WorldGrid is a class that represents a grid in the world space.
    /// It provides methods to convert between world space and grid space.
    /// </summary>
    public class WorldGrid{
        public readonly GridData gridData;
        public WorldGrid(GridData gridData) => this.gridData = gridData;

        /// <summary>
        /// Returns the world position of the cell at the given index.
        /// It calculates the position by multiplying the index by the cell size and adding the origin.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * gridData.cellSize +  gridData.origin;

        /// <summary>
        /// Returns the world position of the cell at the given index.
        /// It calculates the position by multiplying the index by the cell size and adding the origin.
        /// </summary>
        /// <param name="v2Int"></param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(Vector2Int v2Int) =>
            GetWorldPosition(v2Int.x, v2Int.y);

        /// <summary>
        /// Checks if the given index is within the bounds of the grid.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool IsIndexWithinBounds(Vector2Int index) =>
            index.x >= 0 && index.y >= 0 && index.x <  gridData.width && index.y <  gridData.height;

        /// <summary>
        /// Converts the given world position to grid space.
        /// It calculates the index by subtracting the origin from the world position and dividing by the cell size.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="elementIndex"></param>
        /// <returns></returns>
        public bool TryToGetElementIndexFromWorldPosition(Vector3 worldPosition, out Vector2Int elementIndex){
            elementIndex = GetElementIndexFromWorldPosition(worldPosition);
            return IsIndexWithinBounds(elementIndex);
        }

        /// <summary>
        /// Checks if the element exists in the given world position.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public bool IsElementExistInWorldPosition(Vector3 worldPosition){
            return IsIndexWithinBounds(GetElementIndexFromWorldPosition(worldPosition));
        }

        /// <summary>
        /// This method converts the given world position to grid space.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public  Vector2Int GetElementIndexFromWorldPosition(Vector3 worldPosition){
            Vector3 localPos = worldPosition -  gridData.origin;
            var x = Mathf.RoundToInt(localPos.x /  gridData.cellSize);
            var y = Mathf.RoundToInt(localPos.y /  gridData.cellSize);
            return new Vector2Int(x, y);
        }
        
        /// <summary>
        /// Returns the center index of the grid.
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetGridCenterIndex()
        {
            var centerX = Mathf.FloorToInt(( gridData.width - 1) / 2f);
            var centerY = Mathf.FloorToInt(( gridData.height - 1) / 2f);
            return new Vector2Int(centerX, centerY);
        }
    }
}