using UnityEngine;

public interface IDamageTrigger // MY FIRST TIME WRITTING AN INTERFACE SO MAY BE WRONG
{
    void DealDamage(Collider other, int amount);
}