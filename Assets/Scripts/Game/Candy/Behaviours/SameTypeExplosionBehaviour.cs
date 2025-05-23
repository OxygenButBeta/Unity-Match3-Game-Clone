﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using Unity.Collections;
using UnityEngine;

namespace Match3{
    public class SameTypeExplosionBehaviour : ICandyBehaviour{
        public async UniTask OnExplodeTask(Match3Board board, GridNode<Candy> selfGridNode){
            var renderer = Object.FindAnyObjectByType<LineRenderer>();
            NativeList<Vector3> positions = new(2, Allocator.Persistent);
            positions.Add(selfGridNode.Item.transform.position);

            foreach (var element in board.Grid.IterateAll()){
                if (element.Index == selfGridNode.Index)
                    continue;

                if (!element.Item.scriptableCandy.Equals(selfGridNode.Item.scriptableCandy)){
                    continue;
                }

                if (!element.Item.IsExploded){
                    element.Item.transform.DOScale(Vector3.one * .75f, 0.5f).onComplete += () => {
                        element.IsFilled = false;
                        element.Item.ExplodeImmediate();
                    };
                    positions.Add(element.Item.transform.position);
                    positions.Add(selfGridNode.Item.transform.position);
                    renderer.positionCount = positions.Length;
                    renderer.SetPositions(positions.ToArray(Allocator.Temp));
                    await UniTask.Delay(100);
                }
            }

            positions.Dispose();
            renderer.enabled = true;
            await UniTask.WaitForSeconds(.5f);
            renderer.enabled = false;
        }
    }
}