using UnityEngine;

public class KeepScale : MonoBehaviour
{
    private Vector3 originalWorldScale;

    private void Awake()
    {
        originalWorldScale = transform.localScale;

    }


    public void ReturnScale()
    {
        if (transform.parent == null) return;

        Vector3 parentScale = transform.parent.localScale;

        transform.localScale = new Vector3(
            parentScale.x, parentScale.y * originalWorldScale.y, parentScale.z
        );
        Debug.Log("Returned scale");
    }
}
