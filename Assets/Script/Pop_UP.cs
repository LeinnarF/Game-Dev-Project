using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Pop_Up : MonoBehaviour
{
    public GameObject popUpPanel; // Reference to the pop-up panel GameObject
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
        else if (collision.CompareTag("Player") == false)
        {
            // Deactivate the pop-up panel
            popUpPanel.SetActive(false);
            anim.SetBool("pop", false);
        }
        else
        {
            Yes();
            No();
        }
    }

    public void Yes()
    {
        Debug.Log("yes");
        anim.SetBool("pop", true);
        
    }
    public void No()
    {
        Debug.Log("no");
        anim.SetBool("pop", true);
    }
}
