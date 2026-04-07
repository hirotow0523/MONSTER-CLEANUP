using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float Timer = 60f; // 戦闘時間
    public Text TimerText;
    public Text CountText;
    public GameObject ClearWindow;

    public PlayerController player;

    public AudioSource bgmSource;
    public AudioClip battleBGM;
    
    bool gameStart = false; 
    public static bool gameEnd = false;
    bool isSEPlayed = false;

    void Start()
    {
        player.enabled = false;
        gameEnd = false;
        StartCoroutine(StartCountdown());
    }

    // ゲームスタートのカウントダウン
    IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);
        
        int count = 3;
        bool isCountSEPlayed = false;

        while (count > 0)
        {
            CountText.text = count.ToString();

            if (!isCountSEPlayed)
            {
                SEManager.Instance.PlayCountDown();
                isCountSEPlayed = true;
            }            
            
            yield return new WaitForSeconds(1f);
            count--;
        }

        CountText.text = "Start!";
        yield return new WaitForSeconds(1f);
        CountText.gameObject.SetActive(false);

        gameStart = true;
        player.enabled = true; // プレイヤー操作開始

        bgmSource.clip = battleBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // 0秒になったらゲームクリア
    void Update()
    {
        if (!gameStart || gameEnd) return;

        Timer -= Time.deltaTime;
        TimerText.text = ((int)Timer).ToString();

        if (Timer <= 0 && !isSEPlayed)
        {
            GameClear();
            isSEPlayed = true;
        }
    }

    // ゲームクリア処理
    void GameClear()
    {
        gameEnd = true;

        player.enabled = false; // プレイヤー操作停止
        player.PlayerAnimator.SetBool("run", false);
        player.SetLayerRecursively(gameObject, LayerMask.NameToLayer("PlayerInvincible"));
        SEManager.Instance.PlayGameEnd();
        ClearWindow.SetActive(true);
    }

    public void GameOver()
    {
        gameEnd = true;

        player.enabled = false; // プレイヤー操作停止
    }
}