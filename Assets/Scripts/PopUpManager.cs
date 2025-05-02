using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    // enables a pop up canvas for 3 seconds
    public IEnumerator ShowNotification(string message) 
    {
        gameObject.transform.GetComponentInChildren<TMP_Text>().SetText(message);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(3); // source: https://docs.unity3d.com/6000.0/Documentation/Manual/coroutines.html
        gameObject.SetActive(false);
    }
}
