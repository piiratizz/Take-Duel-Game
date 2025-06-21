using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimatorEventsHandler : MonoBehaviour
{
    public readonly UnityEvent StepEvent = new UnityEvent();

    public void OnStep() => StepEvent.Invoke();
}