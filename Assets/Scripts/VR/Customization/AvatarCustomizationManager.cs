using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCustomizationManager : MonoBehaviour
{
    public void RepositionUI(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }
}
