using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI _amountText;
    public TextMeshProUGUI _additionalText;

    public AudioClip audioClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        var characterInventory = FindObjectOfType<CharacterInventory>();
        characterInventory.CoinsCollected += OnCoinsCollected;
        _amountText.text = "0";
        _additionalText.alpha = 0f;
    }

    private void OnCoinsCollected(int prev, int cur)
    {
        audioSource.PlayOneShot(audioClip);

        _additionalText.text = $"+{cur - prev}";
        _additionalText.alpha = 1f;
        _additionalText.rectTransform.anchoredPosition = new Vector2(-380f,_additionalText.rectTransform.anchoredPosition.y);
        _additionalText.DOFade(0f, 0.05f).SetDelay(0.15f);
        _additionalText.rectTransform.DOAnchorPosX(-120f, 0.2f).SetEase(Ease.InQuint).onComplete =
            () =>
            {
                _amountText.text = cur.ToString();
                _amountText.rectTransform.DOPunchScale(Vector3.one * 1.1f, 0.1f);
            };
    }
}