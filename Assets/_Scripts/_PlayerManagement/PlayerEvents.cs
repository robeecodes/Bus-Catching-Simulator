using System;
using System.Collections;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    [SerializeField] private Transform playerHeadTransform;
    [SerializeField] private Camera playerCamera;

    private SmokeScreen _smokeScreen;

    private void Awake()
    {
        _smokeScreen = GetComponent<SmokeScreen>();
    }

    private void Update()
    {
        if (GameManager.Instance.isScreenSmoke)
        {
            StartCoroutine(ClearSmoke());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPCController>(out NPCController npcController))
        {
            if (IsInMainCameraView.IsInView(npcController.transform.position, playerCamera))
            {
                npcController.Activate();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<NPCController>(out NPCController npcController))
        {
            if (IsInMainCameraView.IsInView(npcController.transform.position, playerCamera))
            {
                npcController.Activate();
            }
        }
    }

    private IEnumerator ClearSmoke()
    {
        // Smoke will clear automatically after 7 seconds
        yield return new WaitForSeconds(7);
        _smokeScreen.SmokeDown();
        GameManager.Instance.isScreenSmoke = false;
    }
}