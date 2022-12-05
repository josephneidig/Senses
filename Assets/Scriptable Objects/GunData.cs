using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;

    public void upgradeAmmo(int amount)
    {
        magSize += amount;
    }

    public void upgradeDamage(float amount)
    {
        damage += amount;
    }

    public void upgradeReload(float amount)
    {
        reloadTime -= amount;
        reloadTime = Mathf.Min(1f, reloadTime);
    }
}
