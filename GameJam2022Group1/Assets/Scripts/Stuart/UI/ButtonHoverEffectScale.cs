using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StuartH
{
    
    /// <summary>
    ///ButtonHoverEffectScale - Use to tween scale for hover over buttons
    /// </summary>
public class ButtonHoverEffectScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleAmount = 1.1f;
    [SerializeField] private float scaleDuration = 0.2f;
		
    public void OnPointerEnter(PointerEventData eventData) => Hover();
    private void Hover()=>transform.DOScale(Vector3.one * scaleAmount, scaleDuration);
    public void OnPointerExit(PointerEventData eventData) => UnHover();
    private void UnHover()=>transform.DOScale(Vector3.one, scaleDuration);
    
}
}
