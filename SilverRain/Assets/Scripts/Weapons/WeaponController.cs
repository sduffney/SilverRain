using System.Collections;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{

    //Attack(){

    //OnActivate()
    //\Instantiate the gun
    //Start cooldown

    //OnCooldown() {
    //waitforSeconds(cooldown)
    //OnDuration()

    //OnDuration(){
    //Attack()
    //OnCooldown()

    public abstract void OnActivate();
    public abstract IEnumerator OnDuration();
    public abstract IEnumerator OnCoolDown();
    public abstract void Attack();
}
