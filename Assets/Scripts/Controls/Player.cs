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
    }


    public enum State { Idle, Dashing, Hurting };
    [Header("States")]
    public State state;
    //public bool Ready;
    //public bool Locked;
    //public bool Invunerable;

    [Header("Values")]
    public int hp;
    public int maxHp;
    public float stamina;
    public float maxStamina;

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
    public PlayerInput control;
    public Rigidbody2D body;
    public PlayerSetter setter;
    public SpriteRenderer sprite;
    public Animator anim;
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
                (command as AttackCommand).OnAttack.AddListener((mod as AttackMod).OnShot);
                (command as AttackCommand).OnEndAttack.AddListener((mod as AttackMod).AfterShot);
            }
            else if (mod.targetType == CommandType.Movement)
            {
                (command as MoveCommand).OnMove.AddListener((mod as MoveMod).OnMove);
                (command as MoveCommand).OnEndMove.AddListener((mod as MoveMod).OnEndMove);
            }
            else if (mod.targetType == CommandType.Targetting)
            {
                (command as TargetCommand).OnSetTarget.AddListener((mod as TargetMod).OnTarget);
                (command as TargetCommand).OnChangeTarget.AddListener((mod as TargetMod).OnChangeTarget);
            }
        }
    }
    public void RemoveMod(Mods mod)
    {
        mods.Remove(mod);
        mod.Unequip();
        foreach (var command in commands)
        {
            command.ClearListeners();
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
            c.ClearListeners();
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
    public void AddGun(GunData gun)
    {
        var gunInstance = gun;
        if (!gun.testing) gunInstance = Object.Instantiate(gun);
        var gunBase = Instantiate(Resources.Load("Gun") as GameObject, transform).GetComponent<Gun>();
        gunBase.gun = gunInstance;
        gunBase.SetGun();
        foreach (var m in gun.defaultMods)
        {
            AddMod(m);
        }
        guns.Add(gunBase);
    }
    public void RemoveGun(Gun gun)
    {
        guns.Remove(gun);
        foreach (var m in gun.gun.defaultMods)
        {
            RemoveMod(m);
        }
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

    public void SetSpeed(Vector2 speed)
    {
        if (state != State.Idle)
        {
            return;
        }
        var currentX = body.velocity.x;
        var currentY = body.velocity.y;
        var newX = speed.x;
        var newY = speed.y;
        if ((currentX >= 0 && newX >= 0) || (currentX <= 0 && newX <= 0))
        {
            currentX = (Mathf.Abs(currentX) > Mathf.Abs(newX)) ? currentX : newX;
        }
        else
        {
            currentX = currentX + newX;
        }

        if ((currentY >= 0 && newY >= 0) || (currentY <= 0 && newY <= 0))
        {
            currentY = (Mathf.Abs(currentY) > Mathf.Abs(newY)) ? currentY : newY;
        }
        else
        {
            currentY = currentY + newY;
        }
        body.velocity = new Vector2(currentX, currentY);
    }

    TimeDelayer forceTimer;
    bool speedLocked;
    public void ForceSpeed(Vector2 speed, float duration, bool withPriority = false, bool isForce = false, float leftOverSpeed = 0)
    {
        if (!withPriority && speedLocked)
        {
            return;
        }
        if (forceTimer != null)
        {
            forceTimer.Kill();
        }
        speedLocked = true;
        if (isForce)
        {
            body.AddForce(speed);
        }
        else
        {
            body.velocity = speed;
        }
        forceTimer = InstancedAction.DelayAction(() =>
        {
            speedLocked = false;
            if (isForce)
            {
                body.velocity = Vector2.zero;
            }
            else
            {
                body.velocity = speed * leftOverSpeed;
            }
        }, duration);
    }

    public void TakeDamage()
    {

    }

    public void Act(string animationName)
    {
        anim.Play(animationName);
    }
}
