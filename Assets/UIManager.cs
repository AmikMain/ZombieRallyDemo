using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] float fadeAnimationDuration = .3f;

    [SerializeField] CanvasGroup lapCanvasGroup;
    [SerializeField] CanvasGroup garageCanvasGroup;
    [SerializeField] CanvasGroup deathCanvasGroup;

    [SerializeField] TMP_Text lapPercentUIText;
    [SerializeField] TMP_Text zombiesKilledUIText;
    [SerializeField] Image healthImage;
    [SerializeField] TMP_Text lapEndMoneyText;
    private GameStats gameStats;
    private GameManager gameManager;
    private Car car;
    private Health carHealth;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameStats = GameStats.Instance;
        gameManager = GameManager.Instance;
        car = FindAnyObjectByType<Car>();
        carHealth = car.GetComponent<Health>();

        gameManager.OnLapStart += HandleLapStart;
        car.OnCarDied += HandleCarDeath;
    }

    void OnDisable()
    {
        gameManager.OnLapStart -= HandleLapStart;
        car.OnCarDied -= HandleCarDeath;
    }

    void Update()
    {
        UpdateLapPercentUI();
        UpdateZombiesKilledUI();
        UpdateHealthUI();
    }

    private void UpdateZombiesKilledUI()
    {
        zombiesKilledUIText.text = gameStats.GetZombiesKilled().ToString();
    }

    private void UpdateLapPercentUI()
    {
        lapPercentUIText.text = Mathf.RoundToInt(gameStats.GetLapPercent()).ToString() + "%"; 
    }

    private void UpdateHealthUI()
    {
        healthImage.fillAmount = Mathf.Lerp(0f, 1f, (float)carHealth.GetCurrentHealth() / (float)carHealth.GetMaxHealth());       
    }



    private void HandleLapStart()
    {
        StartFadeCoroutine(lapCanvasGroup, 0, 1, fadeAnimationDuration, 2f);      
    }

    private void HandleCarDeath()
    {
        StartFadeCoroutine(lapCanvasGroup, 1, 0, fadeAnimationDuration, 1f);
        StartFadeCoroutine(deathCanvasGroup, 0, 1, fadeAnimationDuration * 3, 2f);


        StartCoroutine(SetLapEndMoneyText());
    }

    public void StartFadeCoroutine(CanvasGroup cg, float start, float end, float duration, float wait)
    {
        StartCoroutine(Fade(cg, start, end, duration, wait));
    }

    private IEnumerator Fade(CanvasGroup cg, float from, float to, float duration, float wait)
    {
        yield return new WaitForSeconds(wait);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
        cg.alpha = to;
    }

    private IEnumerator SetLapEndMoneyText()
    {
        yield return new WaitForSeconds(3f);

        for(int i = 0; i < gameStats.GetLapEndMoney(); i++ )
        {
            yield return null;
            yield return null;
            lapEndMoneyText.text = i.ToString();            
        }

        

    }
}
