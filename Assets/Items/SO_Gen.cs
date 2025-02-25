using Match3;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

#if UNITY_EDITOR
public class SO_Gen : MonoBehaviour{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private VisualEffectAsset vfx;
    [SerializeField] private float defaultTime = .5f;

    [ContextMenu("Gen")]
    void Generate(){
        foreach (var sprite in _sprites){
            ScriptableCandy candy = ScriptableObject.CreateInstance<ScriptableCandy>();
            candy.Set(sprite, vfx);
            AssetDatabase.CreateAsset(candy, "Assets/Items/SO/" + sprite.name+".asset");
        }
    }
}
#endif
