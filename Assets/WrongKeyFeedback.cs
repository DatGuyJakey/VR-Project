using System.Collections;
using UnityEngine;

// Optional script. Place this on the same GameObject as KeySocketDoorUnlock.
// It gives feedback when the wrong key or object is placed in the socket.
public class WrongKeyFeedback : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lockedSound;

    [Header("Visual Message")]
    [Tooltip("A world-space text object saying: This key does not fit.")]
    public GameObject wrongKeyMessage;
    public float messageDuration = 2.5f;

    [Header("Optional Red Flash")]
    public Light redFlashLight;
    public float flashDuration = 0.4f;

    private Coroutine messageRoutine;
    private Coroutine flashRoutine;

    void Start()
    {
        if (wrongKeyMessage != null)
        {
            wrongKeyMessage.SetActive(false);
        }

        if (redFlashLight != null)
        {
            redFlashLight.enabled = false;
        }
    }

    public void PlayFeedback()
    {
        if (audioSource != null && lockedSound != null)
        {
            audioSource.PlayOneShot(lockedSound);
        }

        if (wrongKeyMessage != null)
        {
            if (messageRoutine != null)
            {
                StopCoroutine(messageRoutine);
            }
            messageRoutine = StartCoroutine(ShowMessageRoutine());
        }

        if (redFlashLight != null)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }
            flashRoutine = StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator ShowMessageRoutine()
    {
        wrongKeyMessage.SetActive(true);
        yield return new WaitForSeconds(messageDuration);
        wrongKeyMessage.SetActive(false);
    }

    private IEnumerator FlashRoutine()
    {
        redFlashLight.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        redFlashLight.enabled = false;
    }
}