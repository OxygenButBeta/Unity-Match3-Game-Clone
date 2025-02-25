using Sirenix.OdinInspector;
using UnityEngine;

namespace O2.Grid{
    [System.Serializable]
    public struct GridData{
        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Width of the grid")]
        public int width{ get; private set; }

        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Height of the grid")]
        public int height{ get; private set; }

        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Size of the cell")]
        public float cellSize{ get; private set; }

        [field: SerializeField, TabGroup("Grid Options"), Tooltip("Origin of the grid")]
        public Vector3 origin{ get; private set; }

        public GridData(int width, int height, float cellSize, Vector3 origin){
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;
        }
    }
}