using O2.Grid;
using UnityEngine;

namespace Match3{
    public class Candy : MonoBehaviour{
        public ScriptableCandy scriptableCandy;
        public SpriteRenderer image;
        public bool IsExploded{ get; private set; }
        Vector3 originalScale;

        private void Awake(){
            originalScale = transform.localScale;
        }

        public Candy SetScriptableCandy(ScriptableCandy candySO){
            scriptableCandy = candySO;
            image.sprite = candySO.sprite;
            return this;
        }

        public Candy Explode(Match3Board board, GridElement<Candy> selfGridElement){
            foreach (var candyBehaviour in scriptableCandy.candyBehaviours){
                candyBehaviour.OnExplode(board, selfGridElement);
            }

            gameObject.SetActive(false);
            transform.localScale = originalScale;
            IsExploded = true;
            return this;
        }

        public Candy Reactivate(){
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