using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequence Boost", menuName = "Mods/Attack/SequenceBoost")]
public class SequenceBoost : AttackMod
{
    public int boost;
    public int maxBoosts;
    public float stackDelay;
    public float shotSpeedBonus;
    public bool sequencialDecrease;

    List<Gun> usedGuns = new List<Gun>();
    List<gunSequence> sequences = new List<gunSequence>();

    public override void Equip()
    {
        usedGuns = new List<Gun>();
        sequences = new List<gunSequence>();
    }

    private void OnDisable()
    {
        foreach (var s in sequences)
        {
            if (s.hasBoost)
            {
                s.gun.gun.shotCD += shotSpeedBonus;
                s.hasBoost = false;
            }
        }
    }

    public override void Unequip()
    {
        foreach (var g in usedGuns)
        {
            foreach (var s in sequences)
            {
                if (s.gun == g)
                {
                    g.gun.sequenceShooting -= boost * s.sequence;
                    break;
                }
            }
        }
        usedGuns.Clear();
        sequences.Clear();
    }

    public override void AfterShot(Gun usedGun)
    {
        gunSequence sequence = null;
        if (!usedGuns.Contains(usedGun))
        {
            sequence = new gunSequence(usedGun);
            sequences.Add(sequence);
            usedGuns.Add(usedGun);
            usedGun.gun.shotCD -= shotSpeedBonus;
            sequence.hasBoost = true;
        }
        if (sequence == null)
        {
            foreach (var s in sequences)
            {
                if (s.gun == usedGun)
                {
                    sequence = s;
                    break;
                }
            }
        }
        if (usedGun.shotsFired >= usedGun.gun.magazineSize)
        {
            if(sequence.timer != null)
            sequence.timer.End();
            return;
        }
        if (sequence.timer != null) sequence.timer.Kill();
        sequence.timer = InstancedAction.DelayAction(() =>
        {
            if (sequencialDecrease && sequence.sequence > 1)
            {
                sequence.sequence--;
                sequence.gun.gun.sequenceShooting -= boost;
            }
            else
            {
                sequence.gun.gun.sequenceShooting -= boost * sequence.sequence;
                sequences.Remove(sequence);
                usedGuns.Remove(sequence.gun);
                usedGun.gun.shotCD += shotSpeedBonus;
                sequence.hasBoost = false;
            }
        }, usedGun.shotCD - usedGun.gun.shotCD + stackDelay);
        if (sequence.sequence >= maxBoosts)
        {
            return;
        }
        sequence.sequence++;
        sequence.gun.gun.sequenceShooting += boost;
    }

    public class gunSequence
    {
        public gunSequence(Gun newgun)
        {
            gun = newgun;
            sequence = 0;
        }

        public Gun gun;
        public int sequence;
        public TimeDelayer timer;
        public bool hasBoost = false;
    }
}

