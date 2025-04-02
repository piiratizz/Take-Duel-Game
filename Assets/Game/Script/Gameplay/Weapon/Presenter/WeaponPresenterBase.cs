using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponPresenterBase
{
    private WeaponViewBase _view;
    private WeaponModelBase _model;

    protected WeaponViewBase View => _view;
    protected WeaponModelBase Model => _model;

    protected WeaponPresenterBase(WeaponModelBase model, WeaponViewBase view)
    {
        _model = model;
        _view = view;
    }
    
    public virtual void CmdShoot(NetworkIdentity netIdentity) { }
    public virtual void RpcShoot(NetworkIdentity netIdentity) { }
    public virtual void CmdReload(NetworkIdentity netIdentity) { }
    public virtual void RpcReload(NetworkIdentity netIdentity) { }
}