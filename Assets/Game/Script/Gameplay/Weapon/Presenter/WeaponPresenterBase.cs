using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponPresenterBase
{
    private WeaponViewBase _view;
    private WeaponModelBase _model;

    protected WeaponViewBase View => _view;
    protected WeaponModelBase Model => Model;

    protected WeaponPresenterBase(WeaponModelBase model, WeaponViewBase view)
    {
        _model = model;
        _view = view;
    }
    
    public virtual void Shoot() { }
    public abstract void Reload();
}