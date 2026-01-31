using System;
using UnityEngine;

public class FrontModule : MonoBehaviour, IDamageTrigger
{
    
    public string FRONT_MODULE_LVL_KEY = "FRONT_MODULE_LVL";

    [SerializeField] private FrontModuleData[] modules;
    private float fatalSpeed = 10;
    private bool constantDamage = false; //chainsaw
    public event Action OnFrontModuleUpdated;

    void Awake()
    {
        PlayerPrefs.DeleteAll();   
    }

    public void DealDamage(Collider other, int amount)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(amount, DeathType.Kill);
    }

    void OnTriggerEnter(Collider other)
    {
        if(constantDamage) return;

        if(other.gameObject.CompareTag("Zombie") && GetComponentInParent<Rigidbody>().linearVelocity.magnitude >= fatalSpeed)
        {
            DealDamage(other, 100);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(!constantDamage) return;

        if(other.gameObject.CompareTag("Zombie"))
        {
            DealDamage(other, 100);
        }
    }

    public void SetFrontModule(int frontModuleType)
    {
        FrontModuleData frontModuleData = GetModuleByType(frontModuleType);

        fatalSpeed = frontModuleData.fatalSpeed;
        constantDamage = frontModuleData.constantDamage;

        Instantiate(frontModuleData.prefab, this.gameObject.transform);

        PlayerPrefs.SetInt(FRONT_MODULE_LVL_KEY, frontModuleType);
    }

    public FrontModuleData GetModuleByType(int type)
    {
        return System.Array.Find(modules, m => m.lvl == type); // Analyse
    }

    public void BuyFrontModule()
    {
        int avaliliableMoney = PlayerPrefs.GetInt(GameStats.Instance.COIN_BANK_AMOUNT , 0);

        int currentFrontModuleLevel = PlayerPrefs.GetInt(FRONT_MODULE_LVL_KEY, -1);

        int nextFrontModulePrice = GetNextFrontModulePrice();

        if(nextFrontModulePrice <= avaliliableMoney)
        {
            SetFrontModule(currentFrontModuleLevel + 1);

            int moneyLeft = avaliliableMoney - nextFrontModulePrice;

            PlayerPrefs.SetInt(GameStats.Instance.COIN_BANK_AMOUNT, moneyLeft);

            OnFrontModuleUpdated?.Invoke();
        }
    }

    // Ts is separated cuz also used in another place
    public int GetNextFrontModulePrice()
    {
        Debug.Log(PlayerPrefs.HasKey(FRONT_MODULE_LVL_KEY));
        int currentFrontModuleLevel = PlayerPrefs.GetInt(FRONT_MODULE_LVL_KEY, -1);
        Debug.Log(currentFrontModuleLevel);
        int nextFrontModulePrice = GetModuleByType(currentFrontModuleLevel + 1).price;

        return nextFrontModulePrice;
    }
}



