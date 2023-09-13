using UnityEngine;
using UnityEngine.UI;

public class FadeOutController : MonoBehaviour
{
    private Text textComponent;
    public RewardMovingManager Manager { get; set; } // Reference to the manager

    private void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    public void SetFadeOutText(string text)
    {
        textComponent.text = text;
    }

    public void DeactivateAndReturnToPool()
    {
        gameObject.SetActive(false);
        Manager?.ReturnFadeOutToPool(this.gameObject);
    }
}
