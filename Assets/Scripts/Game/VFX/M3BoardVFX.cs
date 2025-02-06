using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

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
            InitPool();
            board.OnCandySwap += OnCandySwap;
        }

        private void InitPool(){
            explosionVfxPool = new ObjectPool<VisualEffect>(
                createFunc: () => Instantiate(explosionVfxPrefab),
                actionOnGet: (obj) => obj.gameObject.SetActive(true),
                actionOnRelease: (obj) => obj.gameObject.SetActive(false),
                defaultCapacity: 10,
                maxSize: 20
            );
        }

        private void OnCandySwap(GridElement<Candy> firstGridElement, GridElement<Candy> secondGridElement){
            SwipeVFX.SetActive(true);
            SwipeVFX.transform.position = firstGridElement.Item.transform.position;
            SwipeVFX.transform.DOMove(secondGridElement.Item.transform.position, SwipeVfxLifeTime)
                .OnComplete(() => SwipeVFX.SetActive(false));
        }

        public void DirectExplode(Candy candy){
            var explosionVfx = GetExplosionVfx();
            explosionVfx.visualEffectAsset = candy.scriptableCandy.explosionVfx;
            explosionVfx.transform.position = candy.transform.position;
        }

        public void PlayVfx(VisualEffectAsset effect, Vector3 position, float lifeTime){
            var explosionVfx = GetFromPool();
            explosionVfx.visualEffectAsset = effect;
            explosionVfx.transform.position = position;
            ReleaseExplosionVfx(explosionVfx, lifeTime).Forget();
        }

        VisualEffect GetFromPool(){
            if (explosionVfxPool == null)
                InitPool();

            // ReSharper disable once PossibleNullReferenceException
            return explosionVfxPool.Get();
        }

        VisualEffect GetExplosionVfx(){
            var explosionVfx = GetFromPool();
            ReleaseExplosionVfx(explosionVfx, explosionVfxLifeTime).Forget();
            return explosionVfx;
        }

        async UniTaskVoid ReleaseExplosionVfx(VisualEffect explosionVfx, float lifeTime){
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            explosionVfxPool.Release(explosionVfx);
        }
    }
}