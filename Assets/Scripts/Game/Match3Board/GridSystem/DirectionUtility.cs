using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace O2.Grid
{
    /// <summary>
    /// A helper class to convert between Vector2Int and Direction.
    /// </summary>
    public static class DirectionUtility
    {
        /// <summary>
        /// The four cardinal directions.
        /// </summary>
        public static readonly Direction[] AllDirections;

        static DirectionUtility() =>
            AllDirections = new[] { Direction.Left, Direction.Right, Direction.Up, Direction.Down };


        /// <summary>
        /// Converts a Vector2Int to a Direction.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Direction FromVector2Int(Vector2Int vector)
        {
            if (vector == Vector2Int.left)
                return Direction.Left;
            if (vector == Vector2Int.right)
                return Direction.Right;
            if (vector == Vector2Int.up)
                return Direction.Up;

            return vector == Vector2Int.down ? Direction.Down : Direction.Left;
        }

        /// <summary>
        /// Converts a Direction to a Vector2Int.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector2Int ToVector2Int(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Vector2Int.left,
                Direction.Right => Vector2Int.right,
                Direction.Up => Vector2Int.up,
                Direction.Down => Vector2Int.down,
                _ => Vector2Int.zero
            };
        }

        /// <summary>
        /// Converts a Direction to a Vector2Int. based on the angle.
        /// Threshold is used to determine the direction.
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="AngleThresholdTolerance"></param>
        /// <returns></returns>
        public static Vector2Int DetermineDirection(float angle, float AngleThresholdTolerance)
        {
            // up
            if (Mathf.Abs(angle) <= AngleThresholdTolerance)
                return Vector2Int.up;

            return angle switch
            {
                // right
                < 0 when Mathf.Abs(angle + 90) <= AngleThresholdTolerance => Vector2Int.right,
                // left
                > 0 when Mathf.Abs(angle - 90) <= AngleThresholdTolerance => Vector2Int.left,
                // down
                _ when Mathf.Abs(Mathf.Abs(angle) - 180) <= AngleThresholdTolerance => Vector2Int.down,
                // default: return zero, if not matched with any condition
                _ => Vector2Int.zero
            };
        }

        /// <summary>
        /// Inverts the direction of the given direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Direction InvertDirection(this Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                _ => Direction.Left
            };
        }

        /// <summary>
        /// Returns the relative direction between two points.
        /// For example, if the second point is to the right of the first point, the direction will be Right.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Direction GetRelativeDirection(Vector2Int first, Vector2Int second) =>
            (second - first).ToDirection();

        /// <summary>
        /// Converts a Vector2Int to a Direction.
        /// Extension method for Vector2Int.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Direction ToDirection(this Vector2Int vector) => FromVector2Int(vector);
    }
}