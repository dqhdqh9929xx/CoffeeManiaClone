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

    private void Awake()
    {
        col = GetComponent<Collider2D>();
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
                SlotsManager.Instance.AddTile(this);
                GamePlay.Instance.AddFoodFromTile(corlor, type);
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
}