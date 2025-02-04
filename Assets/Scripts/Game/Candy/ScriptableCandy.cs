using System;
using UnityEngine;

namespace Match3{
    [CreateAssetMenu(menuName = "Match Three", fileName = "New Candy")]
    public class ScriptableCandy : ScriptableObject{
        [SerializeReference] public ICandyBehaviour[] candyBehaviours = Array.Empty<ICandyBehaviour>();
        public Sprite sprite;
    }
}