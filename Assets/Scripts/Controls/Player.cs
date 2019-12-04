using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        setter.Set();
        guns.Clear();
        var equippedGuns = GetComponentsInChildren<Gun>();
        foreach (var g in equippedGuns)
        {
            guns.Add(g);
        }
    }

    [Header("Locks")]
    public bool Ready;
    public bool Locked;
    public bool Invunerable;

    [Header("Values")]
    public int hp;
    public int stamina;

    public bool needTarget
    {
        get
        {
            foreach (var g in guns)
            {
                if (g.target == null)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Header("References")]

    public List<Gun> guns = new List<Gun>();
    public PlayerControl control;
    public PlayerSetter setter;
    [HideInInspector] public List<Command> commands = new List<Command>();
    [HideInInspector] public List<Mods> mods = new List<Mods>();

    public void AddMod(Mods mod)
    {
        mods.Add(mod);
        mod.Equip();
        foreach (var command in commands)
        {
            if (command.type != mod.targetType)
            {
                continue;
            }
            if (mod.targetType == CommandType.Attacking)
            {
                (command as AttackCommand).OnDo.AddListener((mod as AttackMod).OnShot);
                (command as AttackCommand).OnEnd.AddListener((mod as AttackMod).AfterShot);
            }
            else if (mod.targetType == CommandType.Movement)
            {
                (command as MoveCommand).OnDo.AddListener((mod as MoveMod).OnMove);
                (command as MoveCommand).OnEnd.AddListener((mod as MoveMod).OnEndMove);
            }
            else if (mod.targetType == CommandType.Targetting)
            {
                (command as TargetCommand).OnDo.AddListener((mod as TargetMod).OnTarget);
                (command as TargetCommand).OnEnd.AddListener((mod as TargetMod).OnChangeTarget);
            }
            else if (mod.targetType == CommandType.Other)
            {
                //(command as AttackCommand).OnDo.AddListener((mod as AttackMod).OnAttack);
            }
        }
    }
    public void RemoveMod(Mods mod)
    {
        mods.Remove(mod);
        mod.Unequip();
        foreach (var command in commands)
        {
            command.OnDo.RemoveAllListeners();
            command.OnEnd.RemoveAllListeners();
        }
        var templist = new List<Mods>();
        foreach (var m in mods)
        {
            templist.Add(m);
        }
        mods.Clear();
        foreach (var m in templist)
        {
            AddMod(m);
        }
    }
    public void AddCommand(Command command)
    {
        command.Equip();
        var tempList = new List<Mods>();
        for (int i = mods.Count - 1; i >= 0; i--)
        {
            tempList.Add(mods[i]);
            RemoveMod(mods[i]);
        }
        commands.Add(command);
        foreach (var m in tempList)
        {
            AddMod(m);
        }
    }
    public void RemoveCommand(Command command)
    {
        command.Unequip();
        foreach (var c in commands)
        {
            c.OnDo.RemoveAllListeners();
            c.OnEnd.RemoveAllListeners();
        }
        commands.Remove(command);
        var templist = new List<Command>();
        foreach (var c in commands)
        {
            templist.Add(c);
        }
        commands.Clear();
        foreach (var c in templist)
        {
            AddCommand(c);
        }
    }
    public void AddGun()
    {

    }
    public void RemoveGun()
    {

    }


    public void SetTarget(Transform target, Gun selectedGun = null)
    {
        if (selectedGun == null)
        {
            if (needTarget)
            {
                foreach (var g in guns)
                {
                    if (g.target == null)
                    {
                        selectedGun = g;
                        break;
                    }
                }
            }
            else
            {
                selectedGun = guns[0];
            }
        }
        guns.Remove(selectedGun);
        selectedGun.SetTarget(target);
        guns.Add(selectedGun);
    }
}
