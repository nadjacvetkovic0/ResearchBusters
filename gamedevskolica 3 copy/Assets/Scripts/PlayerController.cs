using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10.0f;

    private Rigidbody2D rigidBody;

    [SerializeField]
    private GameObject attack;

    [SerializeField]
    private float attackDelay = .5f, attackDuration = .5f, attackDamage = 1f;

    private bool canAttack = true;

    public static TurretNode? CurrentNode = null;
    private bool isOnShop => CurrentNode != null;
    private static bool isInShop = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isInShop = false;
    }

    void Update()
    {
        HandleGracePeriod();
        if (!isInShop)
        {
            HandleMovement();
            HandleAttack();
            HandleTurretNode();
        }
        else
        {
            HandleShop();
        }
    }
    private void HandleMovement()
    {
        var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rigidBody.velocity = moveSpeed * moveInput;
    }
    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack == true)
        {
            //zapocni napad
            canAttack = false;
            attack.SetActive(true);
            StartCoroutine(FinishAttack());
        }
    }
    private void HandleTurretNode()
    {
        if (isOnShop && Input.GetKeyDown(KeyCode.E))
        {
            isInShop = true;
            rigidBody.velocity = Vector2.zero;
            HUDManager.Instance.ToggleShop(isInShop);
        }
    }
    private void HandleShop()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveShop();
        }
    }
    public static void LeaveShop()
    {
        isInShop = false;
        HUDManager.Instance.ToggleShop(isInShop);
    }
    private IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        attack.SetActive(false);
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out Enemy enemy);
        enemy?.AttackEnemy(attackDamage);
        collision.TryGetComponent(out Weakpoint weakpoint);
        weakpoint?.AttackWeakpoint(attackDamage);
        collision.TryGetComponent(out TurretNode node);
        if(node && !node.isOccupied)
        {
            CurrentNode = node;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out TurretNode node);
        if (node)
        {
            CurrentNode = null;
        }
    }
    private void HandleGracePeriod()
    {
        if (GameManager.Instance.isGracePeriod && Input.GetKeyDown(KeyCode.Q))
            GameManager.Instance.isGracePeriod = false;
    }
}
