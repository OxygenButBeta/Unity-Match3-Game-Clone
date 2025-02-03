using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using O2.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3{
    public class Match3Board : ActiveGameBoard<Candy>{
        [SerializeField] ScriptableCandy[] scriptableCandies;
        [SerializeField] Candy prefab;
        [SerializeField, TabGroup("Game Options")] float fallDuration = 0.5f;
        [SerializeField, TabGroup("Game Options")] bool allow2x2Matches = true;
        [SerializeField, TabGroup("Game Options")] bool fallDiagonal = true;

        Vector2Int dragStartIndex;
        Vector2Int dragEndIndex;
        bool ignoreInput;
        GridGravityOptions<Candy> gridGravityOptions;

        #region Trash

        void SetCameraToCenter(){
            var center = grid.GetGridElementAt(width / 2, height / 2);
            var y = center.Item.transform.position.y + center.Item.transform.position.y / 2;
            var camera = Camera.main;
            camera.transform.position = new Vector3(camera.transform.position.x, y, camera.transform.position.z);
        }

        #endregion

        // __ Cache __
        readonly List<UniTask> relocateSpawnedCandiesTasks = new();
        readonly List<UniTask> explodeTasks = new();
        readonly HashSet<Vector2Int> matches = new();
        // __ Cache __

        ScriptableCandy GetRandomCandySO() => scriptableCandies[Random.Range(0, scriptableCandies.Length)];

        protected override void OnAwake(){
            foreach (var element in grid.IterateAll()){
                element.Item = Instantiate(prefab, grid.GetWorldPosition(element), Quaternion.identity, transform)
                    .SetScriptableCandy(GetRandomCandySO());
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
            if (ignoreInput)
                return;

            ignoreInput = true;
            ExecuteSwipe(firstGridElement, secondGridElement).Forget();
        }

        async UniTaskVoid ExecuteSwipe(GridElement<Candy> firstGridElement, GridElement<Candy> secondGridElement){
            // Swap the values and Swap in the visual
            await VisualGridUtilities.SwapWorldPosition(firstGridElement, secondGridElement);
            grid.SwapValues(firstGridElement, secondGridElement);
            FindMatches();

            // Swap back if no match
            if (matches.Count == 0){
                await VisualGridUtilities.SwapWorldPosition(firstGridElement, secondGridElement);
                grid.SwapValues(firstGridElement, secondGridElement);
                ignoreInput = false;
                return;
            }

            await ProcessMatches();
            ignoreInput = false;
        }

        async UniTask ProcessMatches(){
            while (true){
                FindMatches();

                if (matches.Count == 0)
                    break; // No more matches

                await ExplodeMatches();

                // Gravity Simulation
                if (GridSystemUtilities.SimulateGravity(grid, gridGravityOptions))
                    await UniTask.Delay(Convert.ToInt32(fallDuration * 1000)); // Wait for the fall to finish

                await SpawnNewCandies();
            }
        }

        private async UniTask SpawnNewCandies(){
            foreach (GridElement<Candy> element in grid.IterateAll(true)){
                if (!element.Item.IsExploded)
                    continue;

                element.IsFilled = true;
                Vector3 pos = grid.GetWorldPosition(element);
                element.Item.SetScriptableCandy(GetRandomCandySO())
                    .Reactivate()
                    .SetPosition(new Vector3(pos.x, 10 + pos.y, pos.z));

                relocateSpawnedCandiesTasks.Add(element.Item.transform.DOMove(pos, .6f).SetEase(Ease.InOutQuad)
                    .ToUniTask());
            }

            await UniTask.WhenAll(relocateSpawnedCandiesTasks);
            relocateSpawnedCandiesTasks.Clear();
        }

        async UniTask ExplodeMatches(){
            GridElement<Candy>[] elementsToExplode = grid.GetGridElements(matches).ToArray();

            foreach (GridElement<Candy> gridItem in elementsToExplode){
                if (gridItem.Item.IsExploded) // Don't explode static items
                    continue;
                explodeTasks.Add(CreateAnim(gridItem));
            }

            await UniTask.WhenAll(explodeTasks);
            explodeTasks.Clear();
            foreach (GridElement<Candy> gridItem in elementsToExplode){
                gridItem.Item.Explode();
                gridItem.IsFilled = false;
            }
        }

        private UniTask CreateAnim(GridElement<Candy> gridItem){
            return gridItem.Item.transform
                .DOScale(0f, 0.3f)
                .SetEase(Ease.InOutQuad).ToUniTask();
        }


        void FindMatches(){
            matches.Clear();
            for (int i = 0; i < width * height; i++){
                var x = i % width;
                var y = i / width;
                GridElement<Candy> currentElement = grid.GetGridElementAt(x, y);

                // Horizontal
                if (x < width - 2 &&
                    grid.GetGridElementAt(x + 1, y).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x + 2, y).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy)){
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x + 1, y));
                    matches.Add(new Vector2Int(x + 2, y));
                }

                // Vertical
                if (y < height - 2 &&
                    grid.GetGridElementAt(x, y + 1).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x, y + 2).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy)){
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x, y + 1));
                    matches.Add(new Vector2Int(x, y + 2));
                }

                // 2x2 Square
                if (!allow2x2Matches)
                    continue;
                if (x < width - 1 && y < height - 1 &&
                    grid.GetGridElementAt(x + 1, y).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x, y + 1).Item.scriptableCandy.Equals(currentElement.Item.scriptableCandy) &&
                    grid.GetGridElementAt(x + 1, y + 1).Item.scriptableCandy
                        .Equals(currentElement.Item.scriptableCandy)){
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x + 1, y));
                    matches.Add(new Vector2Int(x, y + 1));
                    matches.Add(new Vector2Int(x + 1, y + 1));
                }
            }
        }
    }
}