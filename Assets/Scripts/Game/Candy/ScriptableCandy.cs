using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.VFX;

namespace Match3{
    [CreateAssetMenu(menuName = "Match Three", fileName = "New Candy")]
    public class ScriptableCandy : ScriptableObject{
        [field: SerializeReference]
        public ICandyBehaviour[] candyBehaviours{ get; private set; } = Array.Empty<ICandyBehaviour>();
        [field: SerializeField] public float ExplosionDelay{ get; private set; } = 0.5f;
        [field: SerializeField] public VisualEffectAsset explosionVfx{ get; private set; }
        [field: SerializeField] public Sprite sprite{ get; private set; }

        [Conditional("DEBUG")]
        public void Set(Sprite sprite,VisualEffectAsset asset){

            this.sprite = sprite;
            this.explosionVfx = asset;
        }
    }
}