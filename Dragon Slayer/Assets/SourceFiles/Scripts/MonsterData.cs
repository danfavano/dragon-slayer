using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Dragon Slayer/Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public Sprite idleSprite;
    public Sprite attackSprite;
    public Sprite damageSprite;
    public int maxHealth = 60;
    public bool isLarge;
    
public int attackDamage = 10;
    [TextArea] public string description;
}
