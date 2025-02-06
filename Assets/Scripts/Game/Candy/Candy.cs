using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.VFX;
using O2.Grid;
using UnityEngine;

namespace Match3{
    public class Candy : MonoBehaviour{
        public ScriptableCandy scriptableCandy;
        public SpriteRenderer image;
        public bool IsExploded{ get; private set; }
        Vector3 originalScale;
        readonly List<UniTask> explodeTaskList = new();

        public M3BoardVFX vfxRunner;

        private void Awake(){
            originalScale = transform.localScale;
        }

        public Candy SetScriptableCandy(ScriptableCandy candySO){
            scriptableCandy = candySO;
            image.sprite = candySO.sprite;
            return this;
        }


        public async UniTask ExplodeAsync(Match3Board board, GridElement<Candy> selfGridElement){
            foreach (ICandyBehaviour candyBehaviour in scriptableCandy.candyBehaviours)
                explodeTaskList.Add(candyBehaviour.OnExplodeTask(board, selfGridElement));

            await UniTask.WhenAll(explodeTaskList);
            ExplodeImmediate();
            explodeTaskList.Clear();
        }

        public Candy ExplodeImmediate(){
            if (IsExploded){
                return this;
            }

            vfxRunner.PlayVfx(scriptableCandy.explosionVfx, transform.position, scriptableCandy.ExplosionDelay);
            gameObject.SetActive(false);
            transform.localScale = originalScale;
            IsExploded = true;
            return this;
        }


        public Candy ReActivate(){
            transform.localScale = originalScale;
            gameObject.SetActive(true);
            IsExploded = false;
            return this;
        }

        public Candy SetPosition(Vector3 position){
            transform.position = position;
            return this;
        }
    }
}