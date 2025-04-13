using UnityEngine;

public class IAEye : MonoBehaviour
{
    public DataView dataView = new DataView();
    public Health currentToyInSight;

    void Start()
    {
        dataView.Owner = transform;
        dataView.CreateMesh();
    }

    void Update()
    {
        ScanForToys();
    }

    public void ScanForToys()
    {
        currentToyInSight = null;
        float minDistance = Mathf.Infinity;

        Collider[] toys = Physics.OverlapSphere(transform.position, dataView.distance, dataView.Scanlayers);

        foreach (var toyCollider in toys)
        {
            Health toyHealth = toyCollider.GetComponent<Health>();
            if (toyHealth != null && toyHealth.Entity == Entity.Toy && dataView.IsInSight(toyHealth.AimOffSet))
            {
                float distance = Vector3.Distance(transform.position, toyHealth.transform.position);
                if (distance < minDistance)
                {
                    currentToyInSight = toyHealth;
                    minDistance = distance;
                }
            }
        }
    }
}
