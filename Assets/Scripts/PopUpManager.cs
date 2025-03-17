using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public IEnumerator ShowNotification(string message)
    {
        gameObject.transform.GetComponentInChildren<TMP_Text>().SetText(message);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
