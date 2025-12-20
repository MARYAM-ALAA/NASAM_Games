using UnityEngine;

public class cameraview : MonoBehaviour
{
    public Transform cameraTarget;

    void LateUpdate()
    {
        if (cameraTarget != null)
            transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, transform.position.z);
    }
}
