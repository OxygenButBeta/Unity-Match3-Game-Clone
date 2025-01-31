using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace O2.Extensions{
    public static class VisualGridExtensions{
        public static async UniTask SwapTwoPosition(Transform a, Transform b, Ease ease = Ease.OutBack,
            float duration = 0.5f){
            a.DOMove(b.position, duration).SetEase(ease);
            b.DOMove(a.position, duration).SetEase(ease);
            await UniTask.Delay((int)duration * 1000);
        }
    }
}