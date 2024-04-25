using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "SO/Character/Data")]
public class CharDataSO : ScriptableObject
{
    public string charName;
    public SpriteLibraryAsset _spriteSet;
    public int characterIndex;
    public float moveSpeed = 10;

    
}
