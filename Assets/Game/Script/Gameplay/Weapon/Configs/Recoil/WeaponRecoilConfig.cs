﻿using UnityEngine;

[CreateAssetMenu(menuName = "RecoilConfig/RecoilConfig", fileName = "RecoilConfig", order = 0)]
public class WeaponRecoilConfig : ScriptableObject
{
    public float MoveToNextRecoilPointTime;
    public float SpeedModifier;
    public float VerticalStrength;
    public float HorizontalStrength;
}