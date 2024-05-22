using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turret : MonoBehaviour
{
    public TurretStats stats;
    [field: SerializeField]
    public Bullet BulletPrefab { get; private set; }
    private bool canShoot = true;
    private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy target;
    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = stats.range;
        GetComponent<SpriteRenderer>().sprite = stats.sprite;
    }
    private void Update()
    {
        target = FindTarget();
        if (target && canShoot)
        {
            Shoot();
            StartCoroutine(OnAfterShot());
        }
    }
    private void Shoot()
    {
        Instantiate(BulletPrefab, transform.position, Quaternion.identity).Seek(target); //instantiate stvara nov objekat
    }
    private Enemy FindTarget()
    {
        return enemiesInRange.Where(e => stats.TargetedTypes.Contains(e.Stats.EnemyType)).OrderBy(e => Vector2.Distance(e.transform.position, transform.position)).FirstOrDefault();
    }
    private IEnumerator OnAfterShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(stats.shootDelay);
        canShoot = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Enemy enemy);
        if (enemy) enemiesInRange.Add(enemy);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out Enemy enemy);
        if (enemy && enemiesInRange.Contains(enemy))
            enemiesInRange.Remove(enemy);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.range);
        if (target)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
