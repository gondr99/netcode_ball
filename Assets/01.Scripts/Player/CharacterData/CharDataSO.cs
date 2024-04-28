using System.Text;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "SO/Character/Data")]
public class CharDataSO : ScriptableObject
{
    public Sprite idleSprite;

    public string charName;
    public SpriteLibraryAsset spriteSet;
    public int characterIndex;
    public float moveSpeed = 10;

    public float jumpPower = 10;
    public int jumpCount = 2;

    public int damage = 10;
    public float throwPower = 10;
    public Projectile projectilePrefab;

    public int maxHealth = 80;


    public string GetInfoString()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"Speed      : {moveSpeed}");
        builder.AppendLine();
        builder.Append($"Jump Power : {jumpPower}");
        builder.AppendLine();
        builder.Append($"Jump Count : {jumpCount}");
        builder.AppendLine();
        builder.Append($"Damage     : {damage}");
        builder.AppendLine();
        builder.Append($"Throw Power: {throwPower}");
        builder.AppendLine();
        builder.Append($"Max Health : {maxHealth}");

        return builder.ToString();
    }
}
