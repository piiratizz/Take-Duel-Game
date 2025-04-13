    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class WeaponHolderEventsHandler : MonoBehaviour
    {
        public readonly UnityEvent ReloadCompleteEvent  = new UnityEvent();
        public readonly UnityEvent ReloadStartedEvent  = new UnityEvent();
        
        public void OnReloadComplete()
        {
            Debug.Log("RELOAD COMPLETE");
            ReloadCompleteEvent.Invoke();
        }

        public void OnReloadStarted()
        {
            Debug.Log("RELOAD STARTED");
            ReloadStartedEvent.Invoke();
        }
    }