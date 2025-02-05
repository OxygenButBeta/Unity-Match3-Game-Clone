using O2.Grid;
using UnityEngine;

namespace Match3{
    public class CircleExplosionBehaviour : ICandyBehaviour{
        [SerializeField] int radius = 1;

        public void OnExplode(Match3Board board, GridElement<Candy> selfGridElement){
            Vector2Int gridPos = selfGridElement.Index;

            for (var x = -radius; x <= radius; x++)
            for (var y = -radius; y <= radius; y++){
                Vector2Int targetPos = gridPos + new Vector2Int(x, y);

                if (!board.grid.IsIndexWithinBounds(targetPos))
                    continue;

                if (!(Vector2Int.Distance(gridPos, targetPos) <= radius))
                    continue;

                GridElement<Candy> element = board.grid.GetGridElementAt(targetPos.x, targetPos.y);
                if (element.Index == gridPos)
                    continue;

                if (element.Item.IsExploded)
                    continue;

                element.IsFilled = false;
                element.Item.ExplodeImmediate(true);
            }
        }
    }
}