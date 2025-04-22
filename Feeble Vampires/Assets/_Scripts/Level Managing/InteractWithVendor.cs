using UnityEngine;

public class InteractWithVendor : MonoBehaviour
{
    // Start is called before the first frame update
    public void ActivateVendor()
    {
        FindObjectOfType<Shop>().ActivateVendor();
    }
}
