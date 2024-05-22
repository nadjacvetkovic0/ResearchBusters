using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    
    [Header("CARDS")]
    [SerializeField] private TurretCard cardPrefab;
    [SerializeField] private Transform cardsParent;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private Slider healthSlider;

    [Header("WAVE INFO")]
    [SerializeField] private WaveInfoHUD waveInfoParent;
    [SerializeField] private GroupInfoHUD groupInfoParent;

    [Header("GRACE PERIOD")]
    [SerializeField] private GameObject gracePeriodParent;
    [SerializeField] private Image gracePeriodFill;

    private List<TurretCard> allCards = new List<TurretCard>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetMoneyText(int money) => moneyText.text = $"{money}";
    public void SetWaveCountText(int current, int max) => waveCountText.text = $"WAVE {current}/{max}";

    public void UpdateHealth(float max, float current) =>
        healthSlider.value = current / max;
    private void UpdateGracePeriod(float max, float current) =>
        gracePeriodFill.fillAmount -= (current / max);

    public void CreateCards(List<Turret> turrets)
    {
        foreach(var turret in turrets)
        {
            var card = Instantiate(cardPrefab, cardsParent);
            card.FillCard(turret);
            allCards.Add(card);
        }
    }

    public void ToggleShop(bool toOpen)
    {
        cardsParent.gameObject.SetActive(toOpen);
        var selectedCard = toOpen ? allCards[0].gameObject : null;
        EventSystem.current.SetSelectedGameObject(selectedCard);
    }

    public IEnumerator StartGracePeriod(float duration, WaveList wave)
    {
        gracePeriodFill.fillAmount = 1;
        gracePeriodParent.gameObject.SetActive(true);
        StartCoroutine(ToggleWaveInfo(wave, true));

        float max = duration + Time.deltaTime;
        for(;duration>=0; duration -= Time.deltaTime)
        {
            if (!GameManager.Instance.isGracePeriod)
            {
                gracePeriodParent.gameObject.SetActive(false);
                StartCoroutine(ToggleWaveInfo(wave, false));
                yield break;
            }
            UpdateGracePeriod(max, Time.deltaTime);
            yield return null;
        }
        gracePeriodParent.gameObject.SetActive(false);
        StartCoroutine(ToggleWaveInfo(wave, false));
    }
    public IEnumerator ToggleWaveInfo(WaveList wave, bool toShow)
    {
        if (!toShow)
        {
            waveInfoParent.DeleteAllItems();
            waveInfoParent.gameObject.SetActive(false);

            yield break;
        }

        waveInfoParent.gameObject.SetActive(true);

        // HAKCINA!!!
        yield return new WaitForSeconds(.00001f);

        foreach (var group in wave.Group)
        {
            Instantiate(groupInfoParent, waveInfoParent.transform).Init(group);
        }
    }
}
