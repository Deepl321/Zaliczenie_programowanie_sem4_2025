using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public float damage = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 20f;
    public GameObject projectilePrefab;
    public float spawnDistance = 1f;
}
