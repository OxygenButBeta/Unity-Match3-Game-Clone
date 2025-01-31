using UnityEngine;

namespace Match3
{
    public class Candy : MonoBehaviour
    {
        public ScriptableCandy scriptableCandy;
        public SpriteRenderer image;

        public bool IsExploded;
        public void SetScriptableCandy(ScriptableCandy scriptableCandy)
        {
            this.scriptableCandy = scriptableCandy;
            image.sprite = scriptableCandy.sprite;
        }
    }
}