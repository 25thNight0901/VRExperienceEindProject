using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialScale;
    public float hoverScaleMultiplier = 1.1f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = initialScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = initialScale;
    }
}