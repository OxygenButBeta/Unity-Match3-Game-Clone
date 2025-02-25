using System;
using System.Collections;
using O2.Grid;
using Match3.VFX;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Match3{
    public class Match3Board : ActiveGameBoard<Candy>{
        public event Action<GridNode<Candy>, GridNode<Candy>> OnCandySwap;
        [SerializeField, TabGroup("Game Options")] float fallDuration = 0.5f;
        [SerializeField, TabGroup("Game Options")] bool allow2x2Matches;
        [SerializeField, TabGroup("Game Options")] bool fallDiagonal;
        [SerializeField, TabGroup("Game Options")] bool spawnNewCandies;
        [SerializeField, TabGroup("Game Options")] private float fallOffset;
        [SerializeField, TabGroup("Game Options")] private float fallDurationDelay;
        [SerializeField, TabGroup("Game Options")] float fallMag = 0.05f;
        [SerializeField, TabGroup("Visual")] float afterExplosionDelay = 0.5f;
        [SerializeField] ScriptableCandy[] scriptableCandies;
        [SerializeField] M3BoardVFX m3BoardVFX;
        [SerializeField] Candy prefab;

        GridGravityOptions<Candy> gridGravityOptions;
        readonly CandyEqualityComparer equalityComparer = new();
        bool moveOnProcess;

        // __ Cache __
        readonly List<UniTask> relocateSpawnedCandiesTasks = new();
        readonly HashSet<Vector2Int> matches = new();

        protected override void OnAwake(){
            foreach (GridNode<Candy> element in Grid.IterateAll()){
                element.Item = Instantiate(prefab, Grid.GetWorldPosition(element), Quaternion.identity, transform)
                    .SetScriptableCandy(GetRandomCandySO());
                element.Item.vfxRunner = m3BoardVFX;
            }

            gridGravityOptions = new GridGravityOptions<Candy>(
                (element) => element.Item.IsExploded,
                (element) => element.Item.transform.DOMove(Grid.GetWorldPosition(element), fallDuration)
                    .SetEase(Ease.OutBack), fallDiagonal);

            moveOnProcess = true;
        }

        private void Start(){
            ProcessMatchesAsync().Forget();
        }

        protected override void ExecuteValidatedMove(GridNode<Candy> firstGridNode,
            GridNode<Candy> secondGridNode){
            if (moveOnProcess)
                return;
            OnCandySwap?.Invoke(firstGridNode, secondGridNode);
            moveOnProcess = true;
            ExecuteSwipeAsync(firstGridNode, secondGridNode).Forget();
        }

        async UniTaskVoid ExecuteSwipeAsync(GridNode<Candy> firstGridNode, GridNode<Candy> secondGridNode){
            // Swap the values and Swap in the visual
            await VisualGridUtilities.SwapWorldPosition(firstGridNode, secondGridNode);
            Grid.SwapValues(firstGridNode, secondGridNode);

            // Swap back if no match
            if (!Grid.FindMatches(in matches, equalityComparer, allow2x2Matches)){
                await VisualGridUtilities.SwapWorldPosition(firstGridNode, secondGridNode);
                Grid.SwapValues(firstGridNode, secondGridNode);
                moveOnProcess = false;
                return;
            }

            await ProcessMatchesAsync();
        }

        async UniTask ProcessMatchesAsync(){
            while (true){
                if (!Grid.FindMatches(matches, equalityComparer, allow2x2Matches))
                    break; // No more matches

                await ExplodeMatchesAsync();
                await UniTask.Delay( // Wait for the explosion to finish
                    TimeSpan.FromSeconds(afterExplosionDelay -
                                         (afterExplosionDelay / 3)));

                // Gravity Simulation
                if (GridSystemUtilities.SimulateGravity(Grid, gridGravityOptions))
                    await UniTask.Delay(Convert.ToInt32(fallDuration * 1000));
                // Wait for the fall to finish

                if (spawnNewCandies)
                    await SpawnNewCandiesAsync();
            }

            moveOnProcess = false;
        }

        private async UniTask SpawnNewCandiesAsync(){
            var iterationCount = 0;
            foreach (GridNode<Candy> element in Grid.IterateAll(true)){
                if (!element.Item.IsExploded)
                    continue;

                iterationCount++;
                element.IsFilled = true;
                Vector3 pos = Grid.GetWorldPosition(element);
                element.Item
                    .SetScriptableCandy(GetRandomCandySO())
                    .ReActivate()
                    .SetPosition(new Vector3(pos.x, 10 + pos.y + (fallOffset * iterationCount), pos.z));

                var _fallDuration = fallDurationDelay;
                var delay = iterationCount * fallMag;

                relocateSpawnedCandiesTasks.Add(
                    element.Item.transform.DOMove(pos, _fallDuration)
                        .SetEase(Ease.InOutQuad)
                        .SetDelay(delay)
                        .ToUniTask()
                );
            }

            await UniTask.WhenAll(relocateSpawnedCandiesTasks);
            relocateSpawnedCandiesTasks.Clear();
        }

        async UniTask ExplodeMatchesAsync(){
            List<UniTask> expTasks = new();
            GridNode<Candy>[] elementsToExplode = Grid.GetGridElements(matches).ToArray();
            foreach (GridNode<Candy> gridItem in elementsToExplode){
                // Don't explode static items
                if (gridItem.Item.IsExploded)
                    continue;
                expTasks.Add(gridItem.Item.ExplodeAsync(this, gridItem));
                gridItem.IsFilled = false;
            }

            await UniTask.WhenAll(expTasks);
        }

        ScriptableCandy GetRandomCandySO() => scriptableCandies[Random.Range(0, scriptableCandies.Length)];
    }
}