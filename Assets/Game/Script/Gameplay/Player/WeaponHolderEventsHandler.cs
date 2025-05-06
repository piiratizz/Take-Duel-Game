    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class WeaponHolderEventsHandler : MonoBehaviour
    {
        public readonly UnityEvent ReloadCompleteEvent  = new UnityEvent();
        public readonly UnityEvent ReloadStartedEvent  = new UnityEvent();
        
        public readonly UnityEvent SwitchWeaponStartedEvent  = new UnityEvent();
        public readonly UnityEvent SwitchWeaponChangeMomentEvent  = new UnityEvent();
        public readonly UnityEvent SwitchWeaponEndedEvent  = new UnityEvent();
        
        public void OnReloadComplete()
        {
            ReloadCompleteEvent.Invoke();
        }

        public void OnReloadStarted()
        {
            ReloadStartedEvent.Invoke();
        }

        public void OnWeaponSwitchStarted()
        {
            SwitchWeaponStartedEvent.Invoke();
        }
        
        public void OnWeaponSwitchEnded()
        {
            SwitchWeaponEndedEvent.Invoke();
        }

        public void OnWeaponChangeMoment()
        {
            SwitchWeaponChangeMomentEvent.Invoke();
        }
    }