using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject Enemy1;
    public GameObject Enemy2;

    public Transform EnemyPlace1; // Enemy1スポーン中心
    public Transform EnemyPlace2; // Enemy2スポーン中心

    public float spawnRange = 5f; // スポーン範囲

    float TimeCount;
    public float spawnInterval = 3f; // スポーン間隔

    public int Count;
    public int MaxCount; // 敵の最大数

    void Update()
    {
        if (MaxCount <= Count)
        {
            return;
        }

        TimeCount += Time.deltaTime;

        if (TimeCount > spawnInterval)
        {
            // ランダム位置（EnemyPlace1周辺）
            Vector3 pos1 = EnemyPlace1.position + new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0,
                Random.Range(-spawnRange, spawnRange)
            );

            Instantiate(Enemy1, pos1, Quaternion.identity);
            Count++;

            // ランダム位置（EnemyPlace2周辺）
            Vector3 pos2 = EnemyPlace2.position + new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0,
                Random.Range(-spawnRange, spawnRange)
            );

            Instantiate(Enemy2, pos2, Quaternion.identity);
            Count++;

            TimeCount = 0;
        }
    }
}