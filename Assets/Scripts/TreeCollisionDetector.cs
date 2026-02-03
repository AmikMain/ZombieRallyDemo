using UnityEngine;

public class TreeCollisionDetector : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] AudioSource collisionAudio;
    [SerializeField] private float treeCoollisionVelocity1 = 5;
    [SerializeField] private float treeCoollisionVelocity2 = 15;

    public void OnTriggerEnter(Collider collision)
    {
        if( collision.gameObject.CompareTag("Tree") && transform.parent.GetComponent<Rigidbody>().linearVelocity.magnitude >= treeCoollisionVelocity1)
        {
            collisionAudio.Play();

            if(transform.parent.GetComponent<Rigidbody>().linearVelocity.magnitude >= treeCoollisionVelocity2)
            {
                health.TakeDamage(30, DeathType.Kill); 
            }
        }
    }
}
