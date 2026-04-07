using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("移動速度")]
    public float EnemySpeed;

    [Header("目的地を変更するまで")]
    public float ChangeTime;

    [Header("攻撃可能範囲")]
    public float AtDistance = 2f;

    public Animator anim;
    public Rigidbody rb;
    public Collider WeaponColl;
    public StatusManager StatusManager;
    GameObject Target;

    bool isWalk;
    float Timer;
    float SetRot;
    float rotSpeed = 4;
    float attackCount = 0;
    public float AttackInterval = 2f;

    private void Start()
    {
        // 敵ごとでばらけさせる
        ChangeTime += Random.Range(-0.5f, 1.55f);
        SetRot = Random.Range(0, 360);
        WalkSet(false);      
    }

    void Update()
    {
        if (GameTimer.gameEnd) return; // ゲーム終了時に動きを止める
        
        if (Target != null)
        {
            Chase();
        }
        else
        {
            Search();
        }    
    }

    // Playerを追跡
    void Chase()
    {
        // Playerのいる方向をセットする
        var _dir = Target.transform.position - transform.position;
        SetRot = Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg;

        DirSet();

        // 攻撃範囲の判定
        float dis = Vector3.Distance(Target.transform.position, transform.position);
        
        if (dis > AtDistance)
        {
            // 追尾
            this.transform.Translate(Vector3.forward * EnemySpeed * Time.deltaTime);
            WalkSet(true);
        }
        else
        {
            // 攻撃開始
            WalkSet(false);
            Attack();
        }
    }

    // 未発見状態の徘徊
    void Search()
    {
        Timer += Time.deltaTime;

        if (ChangeTime <= Timer)
        {
            // 一定時間経過で目的地(方向)セット
            Timer = 0;
            SetRot = Random.Range(0, 360);
            WalkSet(false);
        }  
        else if ((ChangeTime / 2) <= Timer)
        {
            // 2〜4秒なら移動
            this.transform.Translate(Vector3.forward * EnemySpeed * Time.deltaTime);
            WalkSet(true);
        }
        else
        {
            // 0〜2秒でその方向を向く
            DirSet();
        }
    }

    // 攻撃開始
    void Attack()
    {
        attackCount += Time.deltaTime;
        if (attackCount >= AttackInterval)
        {
            attackCount = 0;

            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                anim.SetTrigger("attack");
            }
            else
            {
                anim.SetTrigger("attack2");
            }
        }
    }

    // Playerの捕捉判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Target = null;
        }
    }

    // 歩き状態制御
    void WalkSet(bool _isWalk)
    {
        if (isWalk != _isWalk)
        {
            anim.SetBool("walk", _isWalk);
            isWalk = _isWalk;
        }
    }

    // 移動する方向をセット
    void DirSet()
    {
        // 滑らかにその方向を向く
        var _angle = Mathf.LerpAngle(transform.eulerAngles.y, SetRot, rotSpeed * Time.deltaTime);
        var _rot = Vector3.zero;
        _rot.y = _angle;
        transform.eulerAngles = _rot;
    }
}