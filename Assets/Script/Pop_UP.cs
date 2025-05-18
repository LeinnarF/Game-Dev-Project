using TMPro;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
public class Pop_Up : MonoBehaviour
{
    public GameObject popUpPanel; // Reference to the pop-up panel GameObject
    public GameObject FadeOut;
    public GameObject FadeIn;
    public Animator anim;
    public string text;
    public Text popUptext;

    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("PopUp_close") && stateInfo.normalizedTime >= 1f)
        {
            // PopUp_close animation has finished
            anim.SetBool("pop", false);
            popUpPanel.SetActive(false);
        }
       
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Activate the pop-up panel
            popUpPanel.SetActive(true);
            popUptext.text = text;
        }
        else
        {
            Yes();
            No();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Deactivate the pop-up panel
            anim.SetBool("pop", true);
            StartCoroutine(wait());
            
            
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
         popUpPanel.SetActive(false);
    }

    public void Yes()
    {
        Debug.Log("Player chose: YES (sleep)");
        anim.SetBool("pop", true);
        StartCoroutine(Sleeping());
    }
    IEnumerator Sleeping()
    {
        Debug.Log("Sleeping...");
        GameObject Clone = Instantiate(FadeOut);
        Instantiate(Clone);
        yield return new WaitForSeconds(1.5f);
       GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

    foreach (GameObject obj in allObjects)
    {
            if (obj.name.Contains("Fade Out Canvas") && obj.name.Contains("Clone"))
            {
                Destroy(obj);
            }
             
    }
    if (LightIntensityController.Instance != null)
            {
                LightIntensityController.Instance.Sleep();
            }
            else
            {
                Debug.LogWarning("LightIntensityController.Instance is null!");
            }

    GameObject Clone1 = Instantiate(FadeIn);
        Instantiate(Clone1);
        yield return new WaitForSeconds(1.5f);
    GameObject[] allObjects1 = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj1 in allObjects1)
        {
            if (obj1.name.Contains("Fade In Canvas") && obj1.name.Contains("Clone"))
            {
                
                Destroy(obj1);
            }
        }
    }

    public void No()
    {
        Debug.Log("no");
        anim.SetBool("pop", true);
    }
}
