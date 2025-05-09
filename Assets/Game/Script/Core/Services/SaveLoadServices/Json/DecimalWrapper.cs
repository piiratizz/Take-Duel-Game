using System;

[Serializable]
public class DecimalWrapper
{
    public decimal value;
    
    public DecimalWrapper(decimal value)
    {
        this.value = value;
    }
}