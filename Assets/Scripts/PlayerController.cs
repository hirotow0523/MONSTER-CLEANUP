using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // カメラと移動
    public Transform Camera;
    public float PlayerSpeed;
    public float RotationSpeed;

    Vector3 speed = Vector3.zero;
    Vector3 rot = Vector3.zero;
    public float turnSpeed = 360f;
    public Transform cameraTransform;

    // アニメーション
    public Animator PlayerAnimator;
    bool isRun;

    // 回避設定
    public float DodgeTime = 1f;
    public Collider PlayerCollider;
    public bool isDodging = false;

    // 攻撃設定
    public Collider WeaponCollider;
    public bool canMove = true;
    public List<GameObject> hitEnemies = new List<GameObject>();

    // サウンド
    public AudioSource audioSource;
    public AudioClip AttackSE;

    // コンボ用変数
    int comboCount = 0;
    public bool isAttacking = false;
    bool comboInputBuffer = false;

    void Update()
    {
        Move();
        Rotation();
        Attack();
        Dodge();
        // Debug.Log("canMove: " + canMove);
        Camera.transform.position = transform.position;
    }

    // 移動設定
    void Move()
    {
        if (!canMove) return;
        
        isRun = false;
        
        float h = 0f;
        float v = 0f;
        
        if (Input.GetKey(KeyCode.W)) v += 1;
        if (Input.GetKey(KeyCode.S)) v -= 1;
        if (Input.GetKey(KeyCode.A)) h -= 1;
        if (Input.GetKey(KeyCode.D)) h += 1;
        
        Vector3 inputDir = new Vector3(h, 0, v);
        
        if (inputDir != Vector3.zero)
        {
            isRun = true;
            
            // カメラ基準の方向
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            
            camForward.y = 0;
            camRight.y = 0;
            
            camForward.Normalize();
            camRight.Normalize();
            
            Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
            
            // 移動
            transform.Translate(moveDir.normalized * PlayerSpeed * Time.deltaTime, Space.World);
             
            // 回転
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                turnSpeed * Time.deltaTime
            );
        }
        PlayerAnimator.SetBool("run", isRun);
    }

    // 移動速度・向き・走りフラグをセット
    void MoveSet()
    {
        speed.z = PlayerSpeed;
        transform.eulerAngles = Camera.transform.eulerAngles + rot;
        isRun = true;
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && canMove)
        {
            StartCoroutine(DodgeAction());
        }
    }
    
    IEnumerator DodgeAction()
    {
        isDodging = true;
        canMove = false;

        PlayerAnimator.SetTrigger("dodge");
        PlayerAnimator.ResetTrigger("knockback");

        // 無敵判定
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("PlayerInvincible"));

        yield return new WaitForSeconds(DodgeTime);

        // 元に戻す
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Player"));

        canMove = true;
        isDodging = false;
    }

    public void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    // 視点操作
    void Rotation()
    {
        float rot = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            rot = -RotationSpeed;

        if (Input.GetKey(KeyCode.RightArrow))
            rot = RotationSpeed;

        Camera.transform.Rotate(0f, rot * Time.deltaTime, 0f);
    }
    
    // 攻撃処理
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttacking)
            {
                // 攻撃していない → 1段目開始
                comboCount = 1;
                StartCombo();
            }
            else if (comboCount < 3) // 3回まで連続攻撃可能
            {
                // 攻撃中 → バッファに保存
                comboInputBuffer = true;
            }
        }
    }

    // コンボ
    void StartCombo()
    {
        isAttacking = true;
        canMove = false;

        hitEnemies.Clear();

        PlayerAnimator.SetTrigger("attack" + comboCount);
    }

    // 武器判定を無効にする
    void WeaponOff()
    {
        WeaponCollider.enabled = false;
        
        if (!isAttacking) return;

        if (comboInputBuffer && comboCount < 3)
        {
            comboInputBuffer = false;
            comboCount++;
            StartCombo(); // 次の攻撃へ
        }
        else
        {
            // コンボ終了
            isAttacking = false;
            comboInputBuffer = false;
            comboCount = 0;
            canMove = true;
        }
    }
    
    // 武器判定を有効にする
    void WeaponOn()
    {
        WeaponCollider.enabled = true;
        audioSource.PlayOneShot(AttackSE);
    }
    
    // プレイヤーの移動可否
    void Moving()
    {
        canMove = true;
    }
}