using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BattleHUDController : MonoBehaviour
{
    public enum SpriteState { Idle, Attack, Damage }

    private UIDocument _doc;
    private VisualElement _root;
    private BattleManager _battle;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
        _battle = GetComponent<BattleManager>();
    }

    private void Start()
    {
        _root = _doc.rootVisualElement;
        PopulateEnemyCards();
    }

private void PopulateEnemyCards()
    {
        if (_battle == null || _battle.Enemies == null) return;

        for (int i = 0; i < 3; i++)
        {
            MonsterData data = i < _battle.Enemies.Length ? _battle.Enemies[i] : null;

            var card      = _root.Q<VisualElement>($"enemy-card-{i}");
            var nameLabel = _root.Q<Label>($"enemy-name-{i}");
            var spriteEl  = _root.Q<VisualElement>($"enemy-sprite-{i}");
            var hpFill    = _root.Q<VisualElement>($"enemy-hp-fill-{i}");
            var hpText    = _root.Q<Label>($"enemy-hp-text-{i}");

            if (data == null) continue;

            if (nameLabel != null) nameLabel.text = data.monsterName.ToUpper();

            if (spriteEl != null)
            {
                if (data.idleSprite != null)
                    spriteEl.style.backgroundImage = new StyleBackground(data.idleSprite);
                spriteEl.EnableInClassList("enemy-sprite-large", data.isLarge);
            }

            if (card != null)
                card.EnableInClassList("enemy-card-large", data.isLarge);

            if (hpFill != null) hpFill.style.width = Length.Percent(100);
            if (hpText != null) hpText.text = $"{data.maxHealth} / {data.maxHealth}";
        }
    }

public void SetEnemySprite(int index, SpriteState state)
    {
        if (_battle == null || index >= _battle.Enemies.Length) return;
        MonsterData data = _battle.Enemies[index];
        if (data == null) return;

        var spriteEl = _root?.Q<VisualElement>($"enemy-sprite-{index}");
        if (spriteEl == null) return;

        Sprite s = state switch
        {
            SpriteState.Attack => data.attackSprite,
            SpriteState.Damage => data.damageSprite,
            _                  => data.idleSprite,
        };

        if (s != null)
            spriteEl.style.backgroundImage = new StyleBackground(s);
    }

    public void UpdateEnemyHealth(int index, int currentHp, int maxHp)
    {
        var hpFill = _root?.Q<VisualElement>($"enemy-hp-fill-{index}");
        var hpText = _root?.Q<Label>($"enemy-hp-text-{index}");

        float pct = maxHp > 0 ? (float)currentHp / maxHp * 100f : 0f;
        if (hpFill != null) hpFill.style.width = Length.Percent(pct);
        if (hpText != null) hpText.text = $"{currentHp} / {maxHp}";
    }
}
