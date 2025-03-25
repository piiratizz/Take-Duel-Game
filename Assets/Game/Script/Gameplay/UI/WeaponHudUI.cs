using TMPro;
using UnityEngine;

public class WeaponHudUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _clipAmmoText;
    [SerializeField] private TMP_Text _totalAmmoText;

    public TMP_Text ClipAmmo => _clipAmmoText;
    public TMP_Text TotalAmmo => _clipAmmoText;
}