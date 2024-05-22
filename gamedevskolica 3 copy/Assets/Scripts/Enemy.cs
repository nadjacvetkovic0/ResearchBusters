using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    public int sceneID;
    public EnemyStats Stats;
    protected float health;

    private SpriteRenderer sprite;

    protected Transform[] waypoints;
    protected int currentWaypoint = 0;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        waypoints = GameManager.Instance.waypoints;
        transform.position = waypoints[currentWaypoint].transform.position;
        health = Stats.Health;
    }
    public void Update()
    {
        //ukoliko nismo stigli do kraja nastavi sa movementom
        if (currentWaypoint <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, Stats.Speed * Time.deltaTime); //Vector2.MoveTowards(pocetna pozicija, krajnja pozicija, brzina)
            if (transform.position == waypoints[currentWaypoint].transform.position)
            {
                OnWaypointReached();
            }
        }
        else //stigao sam do kraja putanje
        {
            GameManager.Instance.Health -= Stats.Damage;
            Destroy(gameObject);
        }
    }

    protected virtual void OnWaypointReached() //protected - ja i sve klase koje me nasledjuju, virtual - klasa koja nasledjuje moze da promeni i redefinise sta se desava sa funkcijom
    {
        currentWaypoint++;
    }
    public void AttackEnemy(float damage)
    {
        health -= damage;
        //Debug.Log(health);

        StartCoroutine(AnimateDamage());
        if (health <= 0)
        {
            GameManager.Instance.Coins += Stats.CoinsDropped;           
                    if (this is EnemyBoss){
            SceneManager.LoadScene(sceneID);
            
         }
            Destroy(gameObject);
        }
    }

    private IEnumerator AnimateDamage()
    {
        var color = sprite.color;
        sprite.color = Color.gray;
        yield return new WaitForSeconds(.1f);
        sprite.color = color;
    }
}
