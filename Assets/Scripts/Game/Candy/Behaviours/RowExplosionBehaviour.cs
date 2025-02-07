using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class RowExplosionBehaviour : ICandyBehaviour{
        [SerializeField] private float delay = .5f;

        public async UniTask OnExplodeTask(Match3Board board, GridElement<Candy> selfGridElement){
            // Explode the row 
            Vector2Int gridPos = selfGridElement;
            for (var x = 0; x < board._grid.gridData.width; x++){
                GridElement<Candy> element = board._grid.GetGridElementAt(x, gridPos.y);

                if (element.Index == gridPos){
                    element.Item.transform.DOScale(Vector3.one * .75f, delay);
                    continue;
                }

                Sequence sequence = DOTween.Sequence();

                if (!element.Item.IsExploded){
                    sequence.Append(element.Item.transform.DOScale(Vector3.one * .75f, delay)).Join(
                        element.Item.transform.DOShakeRotation(
                            duration: delay,
                            strength: new Vector3(0, 0, 10),
                            vibrato: 10,
                            fadeOut: false
                        )).onComplete = () => {
                        element.IsFilled = false;
                        element.Item.ExplodeImmediate();
                    };
                }
            }

            await UniTask.WaitForSeconds(delay);
        }
    }
}