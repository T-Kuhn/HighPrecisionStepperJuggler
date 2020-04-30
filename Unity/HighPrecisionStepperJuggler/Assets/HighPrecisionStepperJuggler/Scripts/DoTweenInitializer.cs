using UnityEngine;
using DG.Tweening;

public class DoTweenInitializer : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init();
    }
}
