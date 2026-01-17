using UnityEngine;

public class FrontModule : MonoBehaviour, IDamageTrigger
{
    [SerializeField] private FrontModuleData[] modules;
    private float fatalSpeed = 10;
    private bool constantDamage = false; //chainsaw

    void Start()
    {
        SetFrontModule(FrontModuleType.LVL1);
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

    public void SetFrontModule(FrontModuleType frontModuleType)
    {
        FrontModuleData frontModuleData = GetModuleByType(frontModuleType);

        fatalSpeed = frontModuleData.fatalSpeed;
        constantDamage = frontModuleData.constantDamage;

        Instantiate(frontModuleData.prefab, this.gameObject.transform);

    }

    public FrontModuleData GetModuleByType(FrontModuleType type)
    {
        return System.Array.Find(modules, m => m.frontModuleType == type); // Analyse
    }
}

public enum FrontModuleType
{
    LVL1, LVL2, LVL3
}


