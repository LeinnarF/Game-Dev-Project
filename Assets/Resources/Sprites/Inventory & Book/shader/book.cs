using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private float pageSpeed = 0.5f;
    [SerializeField] private List<Transform> pages;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject forwardButton;

    private int currentPageIndex = 0;
    private bool isRotating = false;
    private Coroutine rotationCoroutine;

    private void Start()
    {
        currentPageIndex = Mathf.Clamp(PlayerPrefs.GetInt("BookPageIndex", 0), 0, pages.Count);
        SetAllPageRotations();
        UpdateButtonStates();
    }

    private void SetAllPageRotations()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            float angle = i < currentPageIndex ? 180f : 0f;
            pages[i].rotation = Quaternion.Euler(0, angle, 0);
            pages[i].SetSiblingIndex(i);
        }

        if (currentPageIndex < pages.Count)
            pages[currentPageIndex].SetAsLastSibling();
    }

    public void NavigateForward()
    {
        if (CanTurnPageForward() && !isRotating)
        {
            if (rotationCoroutine != null)
                StopCoroutine(rotationCoroutine);

            rotationCoroutine = StartCoroutine(RotatePage(currentPageIndex, true));
        }
    }

    public void NavigateBack()
    {
        if (CanTurnPageBackward() && !isRotating)
        {
            if (rotationCoroutine != null)
                StopCoroutine(rotationCoroutine);

            rotationCoroutine = StartCoroutine(RotatePage(currentPageIndex - 1, false));
        }
    }

    private IEnumerator RotatePage(int pageIndex, bool forward)
    {
        if (pageIndex < 0 || pageIndex >= pages.Count)
        {
            Debug.LogError("Invalid page index: " + pageIndex);
            yield break;
        }

        isRotating = true;
        Transform page = pages[pageIndex];
        float startAngle = forward ? 0 : 180;
        float endAngle = forward ? 180 : 0;

        page.SetAsLastSibling();

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * pageSpeed;
            float angle = Mathf.LerpAngle(startAngle, endAngle, t);
            page.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        currentPageIndex += forward ? 1 : -1;
        SetAllPageRotations(); // Re-apply correct rotations for all pages

        UpdateButtonStates();
        isRotating = false;

        PlayerPrefs.SetInt("BookPageIndex", currentPageIndex);
        PlayerPrefs.Save();
    }

    private bool CanTurnPageForward()
    {
        return currentPageIndex < pages.Count;
    }

    private bool CanTurnPageBackward()
    {
        return currentPageIndex > 0;
    }

    public int PagesRemainingForward()
    {
        return pages.Count - currentPageIndex;
    }

    public int PagesRemainingBackward()
    {
        return currentPageIndex;
    }

    private void UpdateButtonStates()
    {
        backButton.SetActive(CanTurnPageBackward());
        forwardButton.SetActive(CanTurnPageForward());
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
