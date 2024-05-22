using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI paceText;
    [SerializeField] private TextMeshProUGUI priceText;
    
    private Turret turret;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            PlayerController.CurrentNode.SpawnTurret(turret);
        });
    }

    private void Update()
    {
        button.interactable = GameManager.Instance.Coins >= turret.stats.price;
    }
    public void FillCard(Turret turret)
    {
        this.turret = turret;
        icon.sprite = turret.stats.sprite;
        nameText.text = turret.stats.name;
        damageText.text = turret.BulletPrefab.stats.damage.ToString();
        rangeText.text = turret.stats.range.ToString();
        paceText.text = turret.stats.shootDelay.ToString();
        priceText.text = turret.stats.price.ToString();
    }


}
