using System;
using O2.Grid;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Pool;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace Match3.VFX{
    public class M3BoardVFX : MonoBehaviour{
        [SerializeField] Match3Board board;
        [SerializeField, TabGroup("VFX")] VisualEffect explosionVfxPrefab;
        [SerializeField, TabGroup("VFX")] float explosionVfxLifeTime = 0.5f;
        [SerializeField, TabGroup("VFX")] GameObject SwipeVFX;
        [SerializeField, TabGroup("VFX")] float SwipeVfxLifeTime = 0.5f;
   
        ObjectPool<VisualEffect> explosionVfxPool;

        private void Awake(){
            SwipeVFX.SetActive(false);
            explosionVfxPool = new ObjectPool<VisualEffect>(
                createFunc: () => Instantiate(explosionVfxPrefab),
                actionOnGet: (obj) => obj.gameObject.SetActive(true),
                actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                defaultCapacity: 10,
                maxSize: 20
            );
            board.OnCandySwap += OnCandySwap;
        }
        private void OnCandySwap(GridNode<Candy> firstGridNode, GridNode<Candy> secondGridNode){
            SwipeVFX.SetActive(true);
            SwipeVFX.transform.position = firstGridNode.Item.transform.position;
            SwipeVFX.transform.DOMove(secondGridNode.Item.transform.position, SwipeVfxLifeTime)
                .OnComplete(() => SwipeVFX.SetActive(false));
        }

        public void PlayVFX(VisualEffectAsset effect, Vector3 position, float lifeTime){
            VisualEffect explosionVfx = explosionVfxPool.Get();
            explosionVfx.visualEffectAsset = effect;
            explosionVfx.transform.position = position;
            ReleaseExplosionVFX(explosionVfx, lifeTime).Forget();
        }
        async UniTaskVoid ReleaseExplosionVFX(VisualEffect explosionVfx, float lifeTime){
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            explosionVfxPool.Release(explosionVfx);
        }
    }
}