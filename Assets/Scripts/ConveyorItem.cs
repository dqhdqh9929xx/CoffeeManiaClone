using UnityEngine;
using DG.Tweening;

public class ConveyorItem : MonoBehaviour
{
    private Tween moveTween;

    public int FoodType { get; private set; }
    public int SlotIndex { get; private set; }

    private Transform[] pathPoints;
    private float moveDuration;

    public void Init(
        int slotIndex,
        Transform[] allPathPoints,
        float duration,
        int foodType)
    {
        FoodType = foodType;
        SlotIndex = 0; // bắt đầu từ đầu băng chuyền
        pathPoints = allPathPoints;
        moveDuration = duration;

        transform.position = pathPoints[0].position;

        MoveStepByStep(slotIndex);
    }

    void MoveStepByStep(int targetSlot)
    {
        if (SlotIndex >= targetSlot)
            return;

        moveTween?.Kill();

        moveTween = transform
            .DOMove(pathPoints[SlotIndex + 1].position, moveDuration / pathPoints.Length)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                SlotIndex++;
                MoveStepByStep(targetSlot);
            });
    }

    public void MoveToSlot(int newSlotIndex)
    {
        SlotIndex = newSlotIndex;

        moveTween?.Kill();

        moveTween = transform
            .DOMove(pathPoints[newSlotIndex].position, 0.25f)
            .SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        moveTween?.Kill();
    }
}