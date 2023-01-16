using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoubleXRGrabInteractable : XRGrabInteractable
{
    [SerializeField]
    private Transform secondAttachTransform;

    protected override void Awake()
    {
        base.Awake();
        selectMode = InteractableSelectMode.Multiple;
    }
    
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (interactorsSelecting.Count == 1)
        {
            base.ProcessInteractable(updatePhase);   
        }
        else if (interactorsSelecting.Count == 2 && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            ProcessDoubleGrip();
        }
    }

    protected override void Grab()
    {
        if (interactorsSelecting.Count == 1)
        {
            base.Grab();
        }
    }

    protected override void Drop()
    {
        if (!isSelected)
        {
            base.Drop();
        }
    }

    private void ProcessDoubleGrip()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondAttach = secondAttachTransform;
        Transform secondHand = interactorsSelecting[1].transform;

        Vector3 directionBetweenHands = secondHand.position - firstHand.position;
        Vector3 directionBetweenAttaches = secondAttach.position - firstAttach.position;
        Quaternion rotationFromAttachToForward = Quaternion.FromToRotation(directionBetweenAttaches, transform.forward);

        Quaternion targetRotation = Quaternion.LookRotation(directionBetweenHands, firstHand.up);

        Vector3 worldDirectionFromHandleToBase = transform.position - firstAttach.position;
        Vector3 localDirectionFromHandleToBase = transform.InverseTransformDirection(worldDirectionFromHandleToBase);

        Vector3 targetPositon = firstHand.position + targetRotation * localDirectionFromHandleToBase;
        
        transform.SetPositionAndRotation(targetPositon, targetRotation * rotationFromAttachToForward);
    }
}
