using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tile : MonoBehaviour
{
    public int type; // có 2 loại tile: 0 = khoanh 4, 1 = khoanh 8
    public int layer;
    public string corlor; // có 4 màu: 0 - đỏ, 1 - hồng, 2 - xanh dương, 3 - vàng

    private bool isInSlot = false;
    private Collider2D col;
    private Coroutine moveRoutine;

    // Bottle slot system
    private Transform[] bottleSlots;
    private int reservedCount = 0;
    private int arrivedCount = 0;
    public int TypeCount => (type == 0) ? 4 : 8;
    public bool IsFull => arrivedCount >= TypeCount;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        CreateBottleSlots();
    }
    public Bounds Bounds
    {
        get { return col.bounds; }
    }

    void OnMouseDown()
    {
        if (isInSlot) return;

        if (!BoardManager.Instance.IsBlocked(this))
        {
            if (SlotsManager.Instance.HasEmptySlot())
            {
                isInSlot = true;
                BoardManager.Instance.RemoveTile(this);
                SlotsManager.Instance.AddTile(this);
                GamePlay.Instance.AddFoodFromTile(corlor, type, this);
            }
            else
            {
                Debug.Log("Không còn slot trống");
            }
        }
        else
        {
            Debug.Log("Tile đang bị đè");
        }
    }

    public void MoveToSlot(Vector3 targetPos)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveRoutine(targetPos));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        float duration = 0.5f;
        float time = 0f;
        Vector3 startPos = transform.position;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            t = 1 - Mathf.Pow(1 - t, 3);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }

    void CreateBottleSlots()
    {
        int count = TypeCount;
        bottleSlots = new Transform[count];

        int cols, rows;
        if (count == 4)
        {
            cols = 2; rows = 2;
        }
        else
        {
            cols = 4; rows = 2;
        }

        float spacingX = 0.5f;
        float spacingY = 0.5f;
        float startX = -(cols - 1) * spacingX / 3f;
        float startY = -(rows - 1) * spacingY / 3f;

        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject slotObj = new GameObject($"BottleSlot_{index}");
                slotObj.transform.SetParent(transform);
                slotObj.transform.localPosition = new Vector3(
                    startX + c * spacingX,
                    startY + r * spacingY,
                    0f
                );
                bottleSlots[index] = slotObj.transform;
                index++;
            }
        }
    }

    public Transform ReserveNextSlot()
    {
        if (reservedCount < bottleSlots.Length)
        {
            Transform slot = bottleSlots[reservedCount];
            reservedCount++;
            return slot;
        }
        return null;
    }

    public void OnBottleArrived()
    {
        arrivedCount++;
        if (arrivedCount >= TypeCount)
        {
            SlotsManager.Instance.RemoveTile(this);
            Destroy(gameObject, 0.5f);
        }
    }
}