using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    public GameObject splashEffect;
    
    private void OnTriggerEnter(Collider c)
    {
        var drownable = c.GetComponent<IDrownable>();

        if (drownable != null)
        {
            drownable.OnDrown();
            Instantiate(splashEffect, c.transform.position, Quaternion.identity);
        }
    }
}
