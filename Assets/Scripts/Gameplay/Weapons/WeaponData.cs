using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon data", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float weaponDamage;
    public Sprite weaponIcon;

}
