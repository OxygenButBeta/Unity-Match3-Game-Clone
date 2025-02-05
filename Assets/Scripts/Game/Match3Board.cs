using System;
using System.Linq;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.VFX;
using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3{
    public class Match3Board : ActiveGameBoard<Candy>{
        public event Action<GridElement<Candy>> OnCandyExplode;
        public event Action<GridElement<Candy>, GridElement<Candy>> OnCandySwap;

        [SerializeField] ScriptableCandy[] scriptableCandies;
        [SerializeField] Candy prefab;
        [SerializeField, TabGroup("Game Options")] float fallDuration = 0.5f;
        [SerializeField, TabGroup("Game Options")] bool allow2x2Matches;
        [SerializeField, TabGroup("Game Options")] bool fallDiagonal;
        [SerializeField, TabGroup("Game Options")] bool spawnNewCandies;

        [SerializeField, TabGroup("Game Options")] private float fallOffset;
        [SerializeField, TabGroup("Game Options")] private float fallDurationDelay;
        [SerializeField, TabGroup("Game Options")] float fallMag = 0.05f;
        [SerializeField, TabGroup("Visual")] float afterExplosionDelay = 0.5f;
        [field: SerializeField] public M3BoardVFX m3BoardVFX{ get; private set; }

        GridGravityOptions<Candy> gridGravityOptions;
        readonly CandyEqualityComparer equalityComparer = new();
        bool moveOnProcess;

        // __ Cache __
        readonly List<UniTask> relocateSpawnedCandiesTasks = new();
        readonly HashSet<Vector2Int> matches = new();

        #region Trash

        void SetCameraToCenter(){
            var center = grid.GetGridElementAt(width / 2, height / 2);
            var y = center.Item.transform.position.y + center.Item.transform.position.y / 2;
            var camera = Camera.main;
            camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
        }

        #endregion


        ScriptableCandy GetRandomCandySO() => scriptableCandies[Random.Range(0, scriptableCandies.Length)];

        protected override void OnAwake(){
            foreach (GridElement<Candy> element in grid.IterateAll()){
                element.Item = Instantiate(prefab, grid.GetWorldPosition(element), Quaternion.identity, transform)
                    .SetScriptableCandy(GetRandomCandySO());
                element.Item.Board = this;
            }

            gridGravityOptions = new GridGravityOptions<Candy>(
                (element) => element.Item.IsExploded,
                (element) => element.Item.transform.DOMove(grid.GetWorldPosition(element), fallDuration)
                    .SetEase(Ease.OutBack), fallDiagonal);


            SetCameraToCenter();
            ProcessMatches().Forget();
        }

        protected override void ExecuteValidatedMove(GridElement<Candy> firstGridElement,
            GridElement<Candy> secondGridElement){
            if (moveOnProcess)
                return;

            OnCandySwap?.Invoke(firstGridElement, secondGridElement);
            moveOnProcess = true;
            ExecuteSwipe(firstGridElement, secondGridElement).Forget();
        }


        async UniTaskVoid ExecuteSwipe(GridElement<Candy> firstGridElement, GridElement<Candy> secondGridElement){
            // Swap the values and Swap in the visual
            await VisualGridUtilities.SwapWorldPosition(firstGridElement, secondGridElement);
            grid.SwapValues(firstGridElement, secondGridElement);

            // Swap back if no match
            if (!grid.FindMatches(in matches, equalityComparer, allow2x2Matches)){
                await VisualGridUtilities.SwapWorldPosition(firstGridElement, secondGridElement);
                grid.SwapValues(firstGridElement, secondGridElement);
                moveOnProcess = false;
                return;
            }

            await ProcessMatches();
            moveOnProcess = false;
        }

        async UniTask ProcessMatches(){
            while (true){
                if (!grid.FindMatches(matches, equalityComparer, allow2x2Matches))
                    break; // No more matches

                ExplodeMatches();
                await UniTask.Delay( // Wait for the explosion to finish
                    TimeSpan.FromSeconds(afterExplosionDelay -
                                         (afterExplosionDelay / 3)));

                // Gravity Simulation
                if (GridSystemUtilities.SimulateGravity(grid, gridGravityOptions))
                    await UniTask.Delay(Convert.ToInt32(fallDuration * 1000));
                // Wait for the fall to finish

                if (spawnNewCandies)
                    await SpawnNewCandies();
            }
        }

        private async UniTask SpawnNewCandies(){
            var iterationCount = 0;
            foreach (GridElement<Candy> element in grid.IterateAll(true)){
                if (!element.Item.IsExploded)
                    continue;

                iterationCount++;
                element.IsFilled = true;
                Vector3 pos = grid.GetWorldPosition(element);
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

        void ExplodeMatches(){
            GridElement<Candy>[] elementsToExplode = grid.GetGridElements(matches).ToArray();
            foreach (GridElement<Candy> gridItem in elementsToExplode){
                // Don't explode static items
                if (gridItem.Item.IsExploded)
                    continue;

                OnCandyExplode?.Invoke(gridItem);
                gridItem.Item.Explode(this, gridItem);
                gridItem.IsFilled = false;
            }
        }
    }
}