using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


[Serializable]
public struct WaveGroup
{
    public Enemy EnemyType;
    public int EnemyCount;
    public float SpawnDelay;
}

[Serializable]
public struct WaveList
{
    public List<WaveGroup> Group;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform[] waypoints;

    [SerializeField]
    private List<WaveList> enemiesPerRound;

    [SerializeField]
    private List<Turret> allTurrets;

    public bool isGracePeriod { get; set; } = false;
    [SerializeField] private float gracePeriodLength = 5f;

    private int coins = 0;
    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            HUDManager.Instance.SetMoneyText(coins);
        }
    }

    [SerializeField] private int maxHealth = 10;
    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth); //limituje health na izmedju 0 i maxHealtha
            HUDManager.Instance.UpdateHealth(maxHealth, health);
            if (health <= 0)
            {
                Debug.LogError("crko si!!!");
                MoveToScene(2);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private IEnumerator Start()
    {
        Health = maxHealth;
        HUDManager.Instance.CreateCards(allTurrets);
        for (int i = 0; i < enemiesPerRound.Count; i++)
        {
            if (i == enemiesPerRound.Count - 1)
            { 
                yield return StartCoroutine(HandleVideo());
            } 
            
            HUDManager.Instance.SetWaveCountText(i + 1, enemiesPerRound.Count);
            yield return StartCoroutine(SpawnWave(enemiesPerRound[i]));
            if (i < enemiesPerRound.Count - 1)
            {
                yield return StartCoroutine(StartGracePeriod(enemiesPerRound[i + 1]));
            }
        }

    }



    public IEnumerator SpawnWave(WaveList wave)
    {


        for (var i = 0; i < wave.Group.Count; i++)
        {

            yield return StartCoroutine(SpawnGroup(wave.Group[i]));

        }


    }

    public IEnumerator SpawnGroup(WaveGroup group)
    {
        for (int i = 0; i < group.EnemyCount; i++)
        {
            Instantiate(group.EnemyType, transform); //napravi enemija i stavi da mu parent bude gamemanager
            yield return new WaitForSeconds(group.SpawnDelay);
        }
    }
    private IEnumerator StartGracePeriod(WaveList wave)
    {
        isGracePeriod = true;

        StartCoroutine(HUDManager.Instance.StartGracePeriod(gracePeriodLength, wave));
        for (float timer = gracePeriodLength; timer >= 0; timer -= Time.deltaTime)
        {
            if (!isGracePeriod)
                yield break;
            yield return null;
        }
        isGracePeriod = false;
    }

    //[SerializeField]
    public VideoPlayer videoPlayer;
    VideoClip tvNews;
    public GameObject videoPlayerParent;
    public IEnumerator HandleVideo()
    {
        videoPlayerParent.gameObject.SetActive(true);
        yield return new WaitForSeconds(.1f);
        videoPlayer.Prepare();
        videoPlayer.Play();
        //videoPlayer.gameObject.SetActive(true);
        Debug.Log(videoPlayer.length);
        yield return new WaitForSeconds((float)videoPlayer.length);
        videoPlayerParent.gameObject.SetActive(false);
    }

    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
