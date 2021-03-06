﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setter", menuName = "Player/Setter")]
public class PlayerSetter : ScriptableObject
{
    //public int startingHp;
    //public int MaxStamina;
    //public int startingStamina;


    public List<Command> commands = new List<Command>();
    public List<Mods> mods = new List<Mods>();
    public List<GunData> startingGuns = new List<GunData>();

    public void Set()
    {
        foreach(var c in commands)
        {
            Player.instance.AddCommand(c);
        }

        foreach(var m in mods)
        {
            Player.instance.AddMod(m);
        }

        foreach(var g in startingGuns)
        {
            Player.instance.AddGun(g);
        }
    }
}
