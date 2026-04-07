using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public GameObject Main;
    public int HP;
    public int MaxHP;
    public int Score;
    public Image HPGage;

    public float ResetTime = 0;

    public GameTimer gameTimer;

    public GameObject Effect;

    public AudioSource audioSource;
    public AudioClip HitSE;

    public Animator anim;

    private Collider col;
    bool isInvincible = false;
    private PlayerController player;


    public string TagName;

    void Start()
    {
        col = GetComponent<Collider>();
        player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        // HPが0になった時の処理
        if (HP <= 0)
        {
            HP = 0;

            // 死亡エフェクト
            var effect = Instantiate(Effect, transform.position, Quaternion.identity);

            GameObject.Find("GameSystem")
                .GetComponent<GameSystemManager>().Score += Score;

            Destroy(effect, 5f);
            Destroy(Main);

            TimerStop();
        }

        // HPが残っている時の処理
        if (HPGage != null)
        {
            float percent = (float)HP / MaxHP;
            HPGage.fillAmount = percent;
        }
    }

    // 攻撃判定時処理
    private void OnTriggerEnter(Collider other) 
    {
        if (isInvincible) return;
        
        if (other.CompareTag(TagName)) 
        {
            // 自分の武器だった場合は無視
            if (other.transform.root == transform.root) return;
            
            Damage();
            StartCoroutine(InvincibleCoroutine());     
            col.enabled = false;
            StartCoroutine(ColliderResetCoroutine());
        } 
    }

    // 無敵時間
    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        
        yield return new WaitForSeconds(0.2f);
        
        isInvincible = false;    
    }

    // ダメージ処理
    void Damage()
    {
        if (audioSource != null && HitSE != null)
        {
            audioSource.PlayOneShot(HitSE);
        }
        
        HP--;

        // プレイヤーのみコンボをリセット
        if (player != null)
        {
            // 攻撃中はノックバックをキャンセル
            if (player.isAttacking)
            {
                player.PlayerAnimator.ResetTrigger("knockback");
            }
            else
            {
                // 攻撃していない時はノックバック
                if (anim != null) anim.SetTrigger("knockback");
                player.canMove = true;
            }
        }
        else
        {
            // 敵のノックバック
            if (anim != null) anim.SetTrigger("knockback");
        }
    }

    // コライダー再有効化
    IEnumerator ColliderResetCoroutine()
    {
        yield return new WaitForSeconds(ResetTime);
        col.enabled = true;
    }

    // プレイヤー死亡時タイマー停止
    void TimerStop()
    {
        if (GetComponentInParent<PlayerController>() == null) return;

        if (gameTimer != null)
        {
            gameTimer.GameOver();
        }
    }
}