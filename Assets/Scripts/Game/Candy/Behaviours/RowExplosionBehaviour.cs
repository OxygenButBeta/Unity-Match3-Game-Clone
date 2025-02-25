using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class RowExplosionBehaviour : ICandyBehaviour{
        [SerializeField] private float delay = .5f;

        public async UniTask OnExplodeTask(Match3Board board, GridNode<Candy> selfGridNode){
            // Explode the row 
            Vector2Int gridPos = selfGridNode;
            for (var x = 0; x < board.Grid.gridData.width; x++){
                GridNode<Candy> node = board.Grid.GetGridElementAt(x, gridPos.y);

                if (node.Index == gridPos){
                    node.Item.transform.DOScale(Vector3.one * .75f, delay);
                    continue;
                }

                Sequence sequence = DOTween.Sequence();

                if (!node.Item.IsExploded){
                    sequence.Append(node.Item.transform.DOScale(Vector3.one * .75f, delay)).Join(
                        node.Item.transform.DOShakeRotation(
                            duration: delay,
                            strength: new Vector3(0, 0, 10),
                            vibrato: 10,
                            fadeOut: false
                        )).onComplete = () => {
                        node.IsFilled = false;
                        node.Item.ExplodeImmediate();
                    };
                }
            }

            await UniTask.WaitForSeconds(delay);
        }
    }
}