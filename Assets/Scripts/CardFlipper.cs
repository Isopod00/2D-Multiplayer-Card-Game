using UnityEngine;

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