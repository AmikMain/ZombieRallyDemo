using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] AudioSource buttonClickSound;
    [SerializeField] AudioMixerGroup audioMixerGroup;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] float fadeAnimationDuration = .3f;

    [SerializeField] CanvasGroup lapCanvasGroup;
    [SerializeField] CanvasGroup garageCanvasGroup;
    [SerializeField] CanvasGroup deathCanvasGroup;
    [SerializeField] CanvasGroup mainMenuCanvasGroup;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] TMP_Text lapPercentUIText;
    [SerializeField] TMP_Text zombiesKilledUIText;
    [SerializeField] Image healthImage;
    [SerializeField] TMP_Text lapEndMoneyText;
    [SerializeField] TMP_Text moneyBankText;

    [SerializeField] TMP_Text armorPriceText;
    [SerializeField] TMP_Text frontModulePriceText;
    [SerializeField] TMP_Text enginePriceText;
    [SerializeField] CinemachineCamera mainMenuCamera;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] float maxMasterDb = 10;
    [SerializeField] float minMasterDb = -10;
    
    
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
        gameManager.OnLapReload += HandleLapReload;
        car.OnCarDied += HandleCarDeath;
        car.GetComponentInChildren<FrontModule>().OnFrontModuleUpdated += UpdateGarageUI;
        car.GetComponentInChildren<Armor>().OnArmorLevelUpdated += UpdateGarageUI;
        car.GetComponentInChildren<CarEngine>().OnEngineUpdated += UpdateGarageUI;

        masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeSliderValueChange);

        UpdateGarageUI();
        SubscribeButtonsToClickSound();
        ApplyAudioMixer();
    }

    private void HandleMasterVolumeSliderValueChange(float arg0)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Lerp(minMasterDb, maxMasterDb, arg0));
    }

    private void ApplyAudioMixer()
    {
        AudioSource[] audioSources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    private void SubscribeButtonsToClickSound()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayClickSound);
        } 
    }

    void OnDisable()
    {
        gameManager.OnLapStart -= HandleLapStart;
        
        gameManager.OnLapReload -= HandleLapReload;

        if(car == null) return;
        car.OnCarDied -= HandleCarDeath;
        car.GetComponentInChildren<FrontModule>().OnFrontModuleUpdated -= UpdateGarageUI;
        car.GetComponentInChildren<Armor>().OnArmorLevelUpdated -= UpdateGarageUI;
        car.GetComponentInChildren<CarEngine>().OnEngineUpdated -= UpdateGarageUI;
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
        StartFadeCoroutine(garageCanvasGroup, 1, 0, fadeAnimationDuration, 0f);
        StartFadeCoroutine(lapCanvasGroup, 0, 1, fadeAnimationDuration, 2f);      
    }

    public void EnterGarageFromMainMenu()
    {
        mainMenuCamera.enabled = false;

        StartFadeCoroutine(mainMenuCanvasGroup, 1, 0, fadeAnimationDuration, 0f);
        StartFadeCoroutine(garageCanvasGroup, 0, 1, fadeAnimationDuration, fadeAnimationDuration);    
    }

    public void EnterMainMenuFromGarage()
    {
        mainMenuCamera.enabled = true;

        StartFadeCoroutine(garageCanvasGroup, 1, 0, fadeAnimationDuration, 0f);
        StartFadeCoroutine(mainMenuCanvasGroup, 0, 1, fadeAnimationDuration, fadeAnimationDuration);
    }

    private void HandleCarDeath()
    {
        StartFadeCoroutine(lapCanvasGroup, 1, 0, fadeAnimationDuration, 1f);
        StartFadeCoroutine(deathCanvasGroup, 0, 1, fadeAnimationDuration * 3, 2f);


        StartCoroutine(SetLapEndMoneyText());
    }

    private void HandleLapReload()
    {
        StartFadeCoroutine(deathCanvasGroup, 1, 0, fadeAnimationDuration, 0);
        StartFadeCoroutine(garageCanvasGroup, 0, 1, fadeAnimationDuration, fadeAnimationDuration + 0.5f);
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
        
        if(to == 0)
        {
            cg.blocksRaycasts = false;
        }

        if(to == 1)
        {
            cg.blocksRaycasts = true;
        }
    }

    private IEnumerator SetLapEndMoneyText()
    {
        yield return new WaitForSeconds(3f);

        GameManager.Instance.canReloadLap = true;

        moneyBankText.text = PlayerPrefs.GetInt(GameStats.Instance.COIN_BANK_AMOUNT, 6767).ToString();

        for(int i = 0; i <= gameStats.GetLapEndMoney(); i++ )
        {
            yield return null;
            yield return null;
            lapEndMoneyText.text = i.ToString();            
        }
    }

    private void UpdateGarageUI()
    {
        moneyBankText.text = PlayerPrefs.GetInt(GameStats.Instance.COIN_BANK_AMOUNT, 0).ToString();

        string frontModulePrice;
        if (FindAnyObjectByType<FrontModule>().GetNextFrontModulePrice() == -1)
        {
            frontModulePrice = "MAX";
        }
        else
        {
            frontModulePrice = FindAnyObjectByType<FrontModule>().GetNextFrontModulePrice().ToString();
        }
        frontModulePriceText.text = frontModulePrice;


        string armorPrice;
        if (FindAnyObjectByType<Armor>().GetNextArmorLevelPrice() == -1)
        {
            armorPrice = "MAX";
        }
        else
        {
            armorPrice = FindAnyObjectByType<Armor>().GetNextArmorLevelPrice().ToString();
        }
        armorPriceText.text = armorPrice;

        string enginePrice;
        if (FindAnyObjectByType<CarEngine>().GetNextEngineLevelPrice() == -1)
        {
            enginePrice = "MAX";
        }
        else
        {
            enginePrice = FindAnyObjectByType<CarEngine>().GetNextEngineLevelPrice().ToString();
        }
        enginePriceText.text = enginePrice;
    }

    public void PlayClickSound()
    {
        buttonClickSound.Play();
    }

    public void LeaveGame()
    {
        Debug.Log("Выход из игры"); // Для проверки в редакторе
        Application.Quit();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Application.Quit();
    }

    public void ToggleSettings()
    {
        if(settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
        }
        else
        {
            settingsMenu.SetActive(true);
        }
    }

    
}
