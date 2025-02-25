using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class CircleExplosionBehaviour : ICandyBehaviour{
        [SerializeField] int radius = 1;
        [SerializeField] private float delay;

        public async UniTask OnExplodeTask(Match3Board board, GridNode<Candy> selfGridNode){
            Vector2Int gridPos = selfGridNode.Index;
            for (var x = -radius; x <= radius; x++)
            for (var y = -radius; y <= radius; y++){
                Vector2Int targetPos = gridPos + new Vector2Int(x, y);

                if (!board.Grid.IsIndexWithinBounds(targetPos))
                    continue;

                if (!(Vector2Int.Distance(gridPos, targetPos) <= radius))
                    continue;

                GridNode<Candy> node = board.Grid.GetGridElementAt(targetPos.x, targetPos.y);
                if (node.Index == gridPos)
                    continue;

                if (node.Item.IsExploded)
                    continue;

                node.Item.transform.DOShakeRotation(delay, 90, 20, 90).onComplete =
                    () => {
                        node.IsFilled = false;
                        node.Item.ExplodeImmediate();
                    };
            }

            await UniTask.WaitForSeconds(delay);
        }
    }
}