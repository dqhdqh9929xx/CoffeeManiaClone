using UnityEngine;
using DG.Tweening;

public class ConveyorItem : MonoBehaviour
{
    private Tween moveTween;

    public void Init(Vector3[] path, float duration, ConveyorManager manager)
    {
        transform.position = path[0];
        transform.rotation = Quaternion.identity;

        moveTween = transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnUpdate(LockZ);
    }

    void LockZ()
    {
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }

    private void OnDisable()
    {
        moveTween?.Kill();
    }
}