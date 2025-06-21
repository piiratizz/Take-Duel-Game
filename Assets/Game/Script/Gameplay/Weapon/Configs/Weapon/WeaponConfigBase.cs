using UnityEngine;

public class WeaponConfigBase : ScriptableObject
{
    public int ClipAmmo;
    public int TotalAmmo;
    public int FireRate;
    public ParticleSystem MuzzleFlash;
    public ParticleSystem PlayerHitEffect;
    public ParticleSystem EnvironmentHitEffect;
    public SoundData ShotSound;
    public SoundData AttachSound;
    public int PlayerDamage;
    public int EnvironmentDamage;
}

[System.Serializable]
public struct SoundData
{
    public AudioClip Clip;
    public float Volume;
}