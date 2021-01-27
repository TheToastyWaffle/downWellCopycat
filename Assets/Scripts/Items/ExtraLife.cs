using UnityEngine;

public class ExtraLife : ItemBase
{
    public override void Equip()
    {
        GameManager.instance.CharacterStats.life.AddModifier(new StatModifier(1, StatModType.Flat, this));
    }

    public override void Unequip()
    {
        GameManager.instance.CharacterStats.life.RemoveAllModifiersFromSource(this);
    }
}
