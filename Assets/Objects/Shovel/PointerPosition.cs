using UnityEngine;

public class PointerPosition : MonoBehaviour
{
    public Transform shovelPoint; // Referência ao shovelpoint
    public Vector3 positionOffset = Vector3.zero; // Offset da posição (X, Y, Z)
    public Vector3 rotationOffset = Vector3.zero; // Offset da rotação (X, Y, Z)

    private void Update()
    {
        if (shovelPoint != null)
        {
            // Aplica o offset de posição
            shovelPoint.position = transform.position + transform.forward * positionOffset.z
                                                         + transform.right * positionOffset.x
                                                         + transform.up * positionOffset.y;

            // Aplica o offset de rotação
            shovelPoint.rotation = transform.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
