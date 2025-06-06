using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData : ICloneable
{
    public string Name;
    public decimal Balance;

    public string SelectedSkin;
    public List<string> AvailableSkins;
    
    public object Clone()
    {
        var newAvailableSkins = new List<string>(AvailableSkins);

        return new PlayerData()
        {
            Name = Name,
            Balance = Balance,
            SelectedSkin = SelectedSkin,
            AvailableSkins = newAvailableSkins
        };
    }
}