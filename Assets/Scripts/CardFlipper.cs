using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public void Flip()
    {
        GameObject background = gameObject.transform.Find("CardBackground").gameObject;

        if (background.activeInHierarchy)
        {
            background.SetActive(false);
        } else if (!background.activeInHierarchy)
        {
            background.SetActive(true);
        }
    }
}