using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stamina Recover", menuName = "Mods/Player/Stamina Recover")]
public class StaminaRecover : PlayerMod
{
    public float value;

    public override void Equip()
    {
        InstancedAction.RegisterUpdate(Recover);
    }

    public override void Unequip()
    {
        InstancedAction.UnRegisterUpdate(Recover);
    }

    public void Recover()
    {
        Player.instance.stamina += value * Time.deltaTime;
        if(Player.instance.stamina > Player.instance.maxStamina)
        {
            Player.instance.stamina = Player.instance.maxStamina;
        }
    }


}
