using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Match3.VFX{
    public class M3BoardVFX : MonoBehaviour{
        [SerializeField] Match3Board board;

        [SerializeField, TabGroup("VFX")] GameObject explosionVfxPrefab;
        [SerializeField, TabGroup("VFX")] float explosionVfxLifeTime = 0.5f;
        [SerializeField, TabGroup("VFX")] GameObject SwipeVFX;
        [SerializeField, TabGroup("VFX")] float SwipeVfxLifeTime = 0.5f;
        ObjectPool<GameObject> explosionVfxPool;

        private void Awake(){
            SwipeVFX.SetActive(false);

            board.OnCandyExplode += OnCandyExplode;
            board.OnCandySwap += OnCandySwap;
            explosionVfxPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(explosionVfxPrefab),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
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

        private void OnCandyExplode(GridElement<Candy> item){
            GetExplosionVfx().transform.position = item.Item.transform.position;
        }

        GameObject GetExplosionVfx(){
            GameObject explosionVfx = explosionVfxPool.Get();
            ReleaseExplosionVfx(explosionVfx).Forget();
            return explosionVfx;
        }

        async UniTaskVoid ReleaseExplosionVfx(GameObject explosionVfx){
            await UniTask.Delay(TimeSpan.FromSeconds(explosionVfxLifeTime));
            explosionVfxPool.Release(explosionVfx);
        }
    }
}