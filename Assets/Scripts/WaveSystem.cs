using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[]          waves;              //현재 스테이지 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner    enemySpawner;
    private int     currentWaveIndex = 0;      //현재 웨이브 인덱스
    public GameObject bell;
    public int offset = 0;

    [SerializeField]
    public static WaveSystem waveSystem = null;
    private void Awake()
    {
        if (waveSystem == null)
        {
            waveSystem = this;
        }
        else if (waveSystem != this)
        {
            Destroy(gameObject);
        }
    }

    //웨이브 정보 출력을 위한 Get 프로퍼티(현재 웨이브, 총 웨이브)
    public int CurrentWave {
        set => currentWaveIndex = Mathf.Max(0, value);//시작이 0이기 때문에 1
        get => currentWaveIndex;
    }

    public int MaxWave => waves.Length;

    public void StartWave()
    {
        bell.GetComponent<Animator>().SetTrigger("Bell");
        //enemySpawner.StartWave(waves[Random.Range(0, MaxWave)]);
        enemySpawner.StartWave(waves[currentWaveIndex]);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayAudio("WaveStart");
        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
        }
        else
        {
            currentWaveIndex = 0;
            offset += 20;
        }
    }

    private void Update()
    {
        if (GameManager.gameManager.spawntime <= 0f)
        {
            StartWave();
            GameManager.gameManager.spawntime = 30;
        }
    }
}

[System.Serializable]//구조체 데이터를 직렬화하는 명령: 메모리상에 존재하는 오브젝트 정보를 stirng or byte 데이터 형태로 변형하는 것(드라이브 저장, 네트워크를 통한 데이터 전송 가능)
//Tip. 직렬화를 사용하면 Inspector View에서 클래스 내부의 변수 정보들을 수정할 수 있음
public struct Wave
{
    public float        spawnTime;              //현재 웨이브 적 생성 주기
    public int          maxEnemyCount;          //현재 웨이브 적 등장 숫자
    public GameObject[] enemyPrefabs;           //현재 웨이브 적 등장 종류
}
