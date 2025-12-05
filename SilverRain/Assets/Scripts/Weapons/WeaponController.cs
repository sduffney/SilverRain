using System.Collections;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public abstract void OnActivate();
    public abstract IEnumerator OnDuration();
    public abstract IEnumerator OnCoolDown();
    public abstract void Attack();
}
