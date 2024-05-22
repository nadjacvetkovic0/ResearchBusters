using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Phase
{
    public Transform WaypointReached;
    public Weakpoint[] WeakpointsToSpawn;
    public WaveList WaveToSpawn;
}
public class EnemyBoss : Enemy
{
    [SerializeField] private List<Phase> phases = new List<Phase>();
    protected override void OnWaypointReached()
    {
        var currentPhase = phases.Find(phase => phase.WaypointReached == waypoints[currentWaypoint]);
        if (!currentPhase.Equals(default(Phase))) ActivatePhase(currentPhase);
        base.OnWaypointReached();
    }
    private void ActivatePhase(Phase phase)
    {
        foreach (var weakpoint in phase.WeakpointsToSpawn)
            weakpoint.Spawn();
        StartCoroutine(GameManager.Instance.SpawnWave(phase.WaveToSpawn));
    }

    private void Update(){
        Debug.Log(health);
       // base.Update();
        if(health<=0){
           // Debug.Log(health);
            SceneManager.LoadScene(3);
        }
        base.Update();
        //super.Update();
    }

    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
