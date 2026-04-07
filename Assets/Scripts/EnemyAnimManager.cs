using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimManager : MonoBehaviour
{
    public EnemyController enemyController;

    //攻撃判定発生
    public void WeaponON()
    {
        enemyController.WeaponColl.enabled = true;

        enemyController.rb.AddRelativeForce
            (Vector3.forward * enemyController.rb.mass * 10, ForceMode.Impulse);
    }

    //攻撃判定無し
    public void WeaponOFF()
    {
        enemyController.WeaponColl.enabled = false;
        enemyController.anim.SetBool("attack", false);
        enemyController.anim.SetBool("attack2", false); 
    }
}
