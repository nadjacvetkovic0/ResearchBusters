using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemy target;
    public BulletStats stats;
    private void Update()
    {
        Chase();
    }
    public void Seek(Enemy enemy)
    {
        target = enemy;
    }
    private void Chase()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
        var dir = target.transform.position - transform.position;
        float distanceThisFrame = stats.speed * Time.deltaTime;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.up = target.transform.position - transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Enemy enemy);
        enemy?.AttackEnemy(stats.damage);
        Destroy(gameObject);
    }
}
