using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// Place this script on the same GameObject as the XR Socket Interactor.
// It checks whether the inserted key is the correct key and then opens the door.
[RequireComponent(typeof(XRSocketInteractor))]
public class KeySocketDoorUnlock : MonoBehaviour
{
    [Header("Correct Key")]
    [Tooltip("Drag the correct key GameObject here.")]
    public GameObject correctKey;

    [Header("Door Animation - Animator Method")]
    [Tooltip("Recommended beginner option. Drag the door Animator here.")]
    public Animator doorAnimator;

    [Tooltip("This must match the Trigger parameter in the Animator.")]
    public string animatorTriggerName = "OpenDoor";

    [Header("Door Animation - Timeline Method")]
    [Tooltip("Optional alternative. Drag a Playable Director here if using Timeline.")]
    public PlayableDirector doorTimeline;

    [Header("Door Blocking Collider")]
    [Tooltip("Drag the door collider or invisible blocker here. It will be disabled when unlocked.")]
    public Collider doorBlocker;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip unlockSound;

    [Header("Optional Escape or Progress Message")]
    public GameObject messageObject;

    [Header("Settings")]
    public bool useAnimator = true;
    public bool useTimeline = false;
    public bool disableKeyGrabWhenInserted = true;
    public bool makeKeyKinematicWhenInserted = true;

    public bool IsUnlocked { get; private set; }

    private XRSocketInteractor socketInteractor;

    void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();

        if (messageObject != null)
        {
            messageObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnObjectPlacedInSocket);
    }

    void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnObjectPlacedInSocket);
    }

    private void OnObjectPlacedInSocket(SelectEnterEventArgs args)
    {
        if (IsUnlocked)
        {
            return;
        }

        GameObject insertedObject = args.interactableObject.transform.gameObject;

        if (insertedObject == correctKey)
        {
            UnlockDoor(insertedObject);
        }
        else
        {
            Debug.Log("Wrong object placed in socket: " + insertedObject.name);

            WrongKeyFeedback feedback = GetComponent<WrongKeyFeedback>();
            if (feedback != null)
            {
                feedback.PlayFeedback();
            }
        }
    }

    private void UnlockDoor(GameObject insertedKey)
    {
        IsUnlocked = true;
        Debug.Log("Correct key used. Door unlocked.");

        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }

        if (disableKeyGrabWhenInserted)
        {
            XRGrabInteractable grab = insertedKey.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.enabled = false;
            }
        }

        if (makeKeyKinematicWhenInserted)
        {
            Rigidbody rb = insertedKey.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (useAnimator && doorAnimator != null)
        {
            doorAnimator.SetTrigger(animatorTriggerName);
        }

        if (useTimeline && doorTimeline != null)
        {
            doorTimeline.Play();
        }

        if (doorBlocker != null)
        {
            doorBlocker.enabled = false;
        }

        if (messageObject != null)
        {
            messageObject.SetActive(true);
        }

        // Stops the same socket from being reused after unlocking.
        correctKey.SetActive(false);
    }
}