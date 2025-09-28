using UnityEngine;
public class NeedleController : MonoBehaviour
{
    public GameObject north;

    void Update()
    {
        if (north != null && transform.parent != null)
        {
            Vector3 northPosition = north.transform.position;
            Vector3 relativeTarget = transform.parent.InverseTransformPoint(northPosition);

            float needleRotation = Mathf.Atan2(relativeTarget.y, relativeTarget.z) * Mathf.Rad2Deg;

            transform.localRotation = Quaternion.Euler(-needleRotation,  0, 0);
        }
    }
}
