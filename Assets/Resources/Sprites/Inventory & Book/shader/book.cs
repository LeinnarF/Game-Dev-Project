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
            float targetAngle = i < currentPageIndex ? 180f : 0f;
            pages[i].localRotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
    }

    public void NavigateForward()
    {
        if (CanTurnPageForward() && !isRotating)
        {
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
            rotationCoroutine = StartCoroutine(RotatePage(currentPageIndex, true));
        }
    }

    public void NavigateBack()
    {
        if (CanTurnPageBackward() && !isRotating)
        {
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
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

        // Bring the current page to front so it flips above others
        page.SetAsLastSibling();

        float startY = forward ? 0f : 180f;
        float endY = forward ? 180f : 0f;

        Quaternion startRotation = Quaternion.Euler(0f, startY, 0f);
        Quaternion endRotation = Quaternion.Euler(0f, endY, 0f);

        float elapsed = 0f;

        while (elapsed < pageSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / pageSpeed);
            page.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        page.localRotation = endRotation;

        currentPageIndex += forward ? 1 : -1;
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, pages.Count);

        isRotating = false;
        rotationCoroutine = null;

        UpdateButtonStates();

        PlayerPrefs.SetInt("BookPageIndex", currentPageIndex);
        PlayerPrefs.Save();
    }

    private bool CanTurnPageForward() => currentPageIndex < pages.Count;

    private bool CanTurnPageBackward() => currentPageIndex > 0;

    public int PagesRemainingForward() => pages.Count - currentPageIndex;

    public int PagesRemainingBackward() => currentPageIndex;

    public void ForceCloseBook()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
        isRotating = false;
        SetAllPageRotations();
    }

    private void OnDisable() => ForceCloseBook();

    private void OnDestroy()
    {
        ForceCloseBook();
        PlayerPrefs.Save();
    }

    private void UpdateButtonStates()
    {
        backButton?.SetActive(CanTurnPageBackward());
        forwardButton?.SetActive(CanTurnPageForward());
    }
}
