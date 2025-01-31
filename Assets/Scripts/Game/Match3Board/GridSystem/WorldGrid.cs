using UnityEngine;

namespace O2.Grid{
    /// <summary>
    /// WorldGrid is a class that represents a grid in the world space.
    /// It provides methods to convert between world space and grid space.
    /// </summary>
    public class WorldGrid{
        // The width and height of the grid.
        public readonly int width;

        public readonly int height;

        // The size of each cell in the grid.
        readonly float cellSize;

        // The origin of the grid.
        readonly Vector3 origin;

        protected WorldGrid(int width, int height, float cellSize, Vector3 origin){
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;
        }

        /// <summary>
        /// Returns the world position of the cell at the given index.
        /// It calculates the position by multiplying the index by the cell size and adding the origin.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * cellSize + origin;

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
            index.x >= 0 && index.y >= 0 && index.x < width && index.y < height;

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

        public bool IsElementExistInWorldPosition(Vector3 worldPosition){
            return IsIndexWithinBounds(GetElementIndexFromWorldPosition(worldPosition));
        }

        public  Vector2Int GetElementIndexFromWorldPosition(Vector3 worldPosition){
            Vector3 localPos = worldPosition - origin;
            var x = Mathf.RoundToInt(localPos.x / cellSize);
            var y = Mathf.RoundToInt(localPos.y / cellSize);
            return new Vector2Int(x, y);
        }
    }
}