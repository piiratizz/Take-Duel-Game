using Action = System.Action;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class RoundTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    public async UniTask StartTimerAsync(float time)
    {
        _timerText.gameObject.SetActive(true);
        
        float currentTime = time;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            _timerText.text = ((int)currentTime).ToString();
            await UniTask.Yield();
        }
        
        _timerText.gameObject.SetActive(false);
    }
}
