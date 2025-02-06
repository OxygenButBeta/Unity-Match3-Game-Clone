using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class ColumnExplosionBehaviour : ICandyBehaviour{
        [SerializeField] private float delay;

        public async UniTask OnExplodeTask(Match3Board board, GridElement<Candy> selfGridElement){
            // Explode the row
            Vector2Int gridPos = selfGridElement;
            for (var y = 0; y < board.grid.height; y++){
                GridElement<Candy> element = board.grid.GetGridElementAt(gridPos.x, y);

              
                if (element.Index == gridPos){
                    element.Item.transform.DOScale(Vector3.one * .75f, delay);
                    continue;
                }

                if (!element.Item.IsExploded){
                    element.Item.transform.DOScale(Vector3.one * .75f, delay).onComplete = () => {
                        element.IsFilled = false;
                        element.Item.ExplodeImmediate();
                    };
                }
            }

            await UniTask.WaitForSeconds(delay);
        }
    }
}