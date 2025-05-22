using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class book : MonoBehaviour
{
    [SerializeField] private float pageSpeed = 0.5f;
    [SerializeField] private List<Transform> pages;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject forwardButton;

    private int index = -1;
    private bool rotate = false;

    private void Start()
    {
        // Guard against empty page list
        if (pages == null || pages.Count == 0)
        {
            Debug.LogWarning("Book: No pages assigned.");
            return;
        }

        InitialState();
    }

    public void InitialState()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (pages[i] != null)
                pages[i].rotation = Quaternion.identity;
        }

        index = -1;

        if (pages.Count > 0 && pages[0] != null)
            pages[0].SetAsLastSibling();

        if (backButton != null) backButton.SetActive(false);
        if (forwardButton != null) forwardButton.SetActive(true);
    }

    public void RotateForward()
    {
        if (rotate || index + 1 >= pages.Count) return;

        index++;
        float angle = 180f;

        ForwardButtonActions();

        if (pages[index] != null)
            pages[index].SetAsLastSibling();

        StartCoroutine(Rotate(angle, true));
    }

    public void ForwardButtonActions()
    {
        if (backButton != null && !backButton.activeInHierarchy)
            backButton.SetActive(true);

        if (forwardButton != null && index == pages.Count - 1)
            forwardButton.SetActive(false);
    }

    public void RotateBack()
    {
        if (rotate || index < 0) return;

        float angle = 0f;

        if (pages[index] != null)
            pages[index].SetAsLastSibling();

        BackButtonActions();

        StartCoroutine(Rotate(angle, false));
    }

    public void BackButtonActions()
    {
        if (forwardButton != null && !forwardButton.activeInHierarchy)
            forwardButton.SetActive(true);

        if (index - 1 < 0 && backButton != null)
            backButton.SetActive(false);
    }

    private IEnumerator Rotate(float angle, bool forward)
    {
        rotate = true;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        float value = 0f;

        while (true)
        {
            value += Time.deltaTime * pageSpeed;

            if (pages[index] != null)
                pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);

            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if (angle1 < 0.1f)
            {
                if (!forward)
                    index--;

                rotate = false;
                yield break;
            }

            yield return null;
        }
    }
}
