using System.Linq;
using Match3;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScriptable", menuName = "LevelScriptable", order = 0)]
public class LevelScriptable : ScriptableObject{
    [TableMatrix(DrawElementMethod = "DrawCandy")] public ScriptableCandy[,] candies;

    private void DrawCandy(ScriptableCandy candy)
    {
        if (candy != null)
        {
            // Yatay düzenleme yapmak için GUILayout kullanıyoruz
            GUILayout.BeginHorizontal(); // Yatay hizalama başlat
            GUILayout.Label(candy.sprite.texture, GUILayout.Width(50)); // Sprite görselini göster
            GUILayout.Label(candy.explosionVfx.name, GUILayout.Width(100)); // VFX adı
            GUILayout.Label(string.Join(", ", candy.candyBehaviours.Select(b => b.GetType().Name).ToArray()), GUILayout.Width(150)); // Candy davranışlarını göster
            GUILayout.EndHorizontal(); // Yatay hizalamayı bitir
        }
    }
}