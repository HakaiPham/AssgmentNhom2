using UnityEngine;
using UnityEngine.UI;

public class ToggleFadeButton : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    private bool isUsingAlternateImage = false;

    // Sprite gốc và sprite thay thế
    public Sprite originalSprite;
    public Sprite alternateSprite;

    void Start()
    {
        // Lấy thành phần Button và Image
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (button != null)
        {
            // Gắn sự kiện khi nhấn nút
            button.onClick.AddListener(ChangeImage);
        }

        // Đặt sprite ban đầu
        if (buttonImage != null && originalSprite != null)
        {
            buttonImage.sprite = originalSprite;
        }
    }

    public void ChangeImage()
    {
        if (buttonImage != null)
        {
            if (isUsingAlternateImage)
            {
                // Quay lại sprite gốc
                buttonImage.sprite = originalSprite;
                isUsingAlternateImage = false;
            }
            else
            {
                // Đổi sang sprite thay thế
                buttonImage.sprite = alternateSprite;
                isUsingAlternateImage = true;
            }
        }
    }
}
