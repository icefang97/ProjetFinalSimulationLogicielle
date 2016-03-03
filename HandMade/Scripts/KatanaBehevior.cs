using UnityEngine;
using System.Collections;

public class KatanaBehevior : MonoBehaviour
{
    public float range; //attack distance
    public float damage;
    public GameObject target;
    float NextAttackTime;

    void Start()
    {
        NextAttackTime = 0;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && NextAttackTime < Time.time)
            {
            NextAttackTime = Time.time + 1;
            Debug.Log("Pressed left click.");
            GetComponent<Animation>().Play();
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range) && hit.collider.GetComponent<IDamageable>() != null)
            {
                if( hit.collider.tag == target.tag)
                {
                    hit.collider.GetComponent<IDamageable>().TakeDamage(damage);
                    print("touche");
                }
               
            }
        }
   }
    }
