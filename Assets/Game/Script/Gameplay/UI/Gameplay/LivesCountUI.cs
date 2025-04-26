using TMPro;
using UnityEngine;
using Zenject;

public class LivesCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _livesCountText;
    
    public void UpdateText(int livesCount)
    {
        _livesCountText.text = livesCount.ToString();
    }
}
