using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Match3{
    [CreateAssetMenu(menuName = "Match Three", fileName = "New Candy")]
    public class ScriptableCandy : ScriptableObject{
        [SerializeReference] public ICandyBehaviour[] candyBehaviours = Array.Empty<ICandyBehaviour>();
        [field: SerializeField] public float ExplosionDelay{ get; private set; } = 0.5f;
        public VisualEffectAsset explosionVfx;
        public Sprite sprite;
    }
}