using UnityEngine;
using System.Collections.Generic;

public enum UnitType
{
}

public class Unit : MonoBehaviour, IAttacker
{

    public Alignment Alignment;
    public int KillScore = 1;

    public WeaponSlot WeaponSlot;

    public Weapon DefaultWeapon;

    public Weapon CurrentWeapon
    { get { return WeaponSlot ? WeaponSlot.CurrentWeapon : DefaultWeapon; } }

    public bool Attacking
    { get { return CurrentWeapon != null && CurrentWeapon.Attacking; } }

    public bool HasTarget
    { get { return CurrentWeapon != null && CurrentWeapon.HasTarget; } }

    public Transform Target
    { get { return CurrentWeapon != null ? CurrentWeapon.Target : null; } }

    [System.NonSerialized]
    public List<Group> Groups = new List<Group>();

    public delegate void GroupEventHandler(Unit unit, Group group);

    public event GroupEventHandler OnAddedToGroup;
    public event GroupEventHandler OnRemovedFromGroup;

    void OnEnable()
    { UnitManager.Instance.Add(this); }

    void OnDisable()
    {
        if (UnitManager.HasInstance)
            UnitManager.Instance.Remove(this);

        ClearGroups();
    }

    public void EquipWeapon(Weapon prefab)
    {
        if (WeaponSlot)
            WeaponSlot.SetWeaponPrefab(prefab);
    }

    public void Die(Attack attack)
    {
        var killer = attack.Weapon ? attack.Weapon.Unit : null;
        if (killer != null)
            killer.Killed(this, attack);
    }

    private void Killed(Unit victim, Attack attack)
    {
    }

    public void SetGroup(Group group)
    {
        ClearGroups();
        group.AddUnit(this);
    }

    public void ClearGroups()
    {
        for (var i = Groups.Count - 1; i >= 0; i--)
            Groups[i].RemoveUnit(this);

        Groups.Clear();
    }

    public void AddedToGroup(Group group)
    {
        Groups.Add(group);
        if (OnAddedToGroup != null)
            OnAddedToGroup(this, group);
    }

    public void RemovedFromGroup(Group group)
    {
        Groups.Remove(group);
        if (OnRemovedFromGroup != null)
            OnRemovedFromGroup(this, group);
    }

}
