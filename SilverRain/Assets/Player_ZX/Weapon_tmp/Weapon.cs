using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    string weaponName = "Weapon";
    public int level = 1;
    public int damage = 10;
    public float cooldown = 1.0f;
    public GameObject weaponModel;
    public GameObject projectilePrefab;
    public AudioClip shootSound;
    public AudioClip attackSound;

}
