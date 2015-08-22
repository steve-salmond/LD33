using UnityEngine;
using System.Collections;

public class WeaponSlot : MonoBehaviour
{

    public Weapon DefaultWeapon;

    public Facing Facing;
    private Unit _unit;

    public Weapon CurrentWeapon
    { get { return _currentWeapon; } }

    private Weapon _currentWeapon;

    private void OnEnable()
    {
        _unit = GetComponentInParent<Unit>();

        if (!_currentWeapon)
            SetWeaponPrefab(DefaultWeapon);
    }

    public Weapon SetWeaponPrefab(Weapon prefab)
    {
        var weapon = ObjectPool.Instance.GetObjectWithComponent<Weapon>(prefab);
        return SetWeapon(weapon);
    }

    public Weapon SetWeapon(Weapon weapon)
    {
        if (_currentWeapon == weapon)
            return _currentWeapon;

        if (_currentWeapon)
            ObjectPool.Instance.ReturnObject(_currentWeapon.gameObject, true);

        _currentWeapon = weapon;
        if (!_currentWeapon)
            return null;

        _currentWeapon.transform.parent = transform;
        _currentWeapon.transform.localPosition = Vector3.zero;
        _currentWeapon.transform.localRotation = Quaternion.identity;
        _currentWeapon.Unit = _unit;
        _currentWeapon.Facing = Facing;

        return _currentWeapon;
    }
}
