using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class CircleExplosionBehaviour : ICandyBehaviour{
        [SerializeField] int radius = 1;
        [SerializeField] private float delay;
        public async UniTask OnExplodeTask(Match3Board board, GridElement<Candy> selfGridElement){
            Vector2Int gridPos = selfGridElement.Index;

            for (var x = -radius; x <= radius; x++)
            for (var y = -radius; y <= radius; y++){
                Vector2Int targetPos = gridPos + new Vector2Int(x, y);

                if (!board._grid.IsIndexWithinBounds(targetPos))
                    continue;

                if (!(Vector2Int.Distance(gridPos, targetPos) <= radius))
                    continue;

                GridElement<Candy> element = board._grid.GetGridElementAt(targetPos.x, targetPos.y);
                if (element.Index == gridPos)
                    continue;

                if (element.Item.IsExploded)
                    continue;

                element.Item.transform.DOShakeRotation(delay, 90, 20, 90).onComplete =
                    () => {
                        element.IsFilled = false;
                        element.Item.ExplodeImmediate();
                    };
            }

            await UniTask.WaitForSeconds(delay);
        }
    }
}