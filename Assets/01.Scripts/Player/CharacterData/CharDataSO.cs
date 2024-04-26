using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "SO/Character/Data")]
public class CharDataSO : ScriptableObject
{
    public Sprite idleSprite;

    public string charName;
    public SpriteLibraryAsset _spriteSet;
    public int characterIndex;
    public float moveSpeed = 10;

    public float jumpPower = 10;
    public int jumpCount = 2;

}
