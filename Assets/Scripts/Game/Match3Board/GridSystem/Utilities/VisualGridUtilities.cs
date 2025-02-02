using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace O2.Grid{
    public static class VisualGridUtilities{
        public static async UniTask SwapWorldPosition<T>(GridElement<T> start, GridElement<T> end,
            Ease ease = Ease.OutBack, float duration = .5f) where T : MonoBehaviour{
            start.Item.transform.DOMove(end.Item.transform.position, duration).SetEase(ease);
            end.Item.transform.DOMove(start.Item.transform.position, duration).SetEase(ease);
            var durationAsMs = Convert.ToInt32(duration * 1000);
            await UniTask.Delay(durationAsMs);
        }
    }
}