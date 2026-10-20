using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Solution
{
    public class ActionHistoryManager : MonoBehaviour
    {
        // 1. Undo System using Stack (LIFO: Last-In, First-Out)
        private Stack<Vector2> undoStack = new Stack<Vector2>();
        private Stack<Vector2> redoStack = new Stack<Vector2>();
        
        // 2. Auto-Move System using Queue (FIFO: First-In, First-Out)
        private Queue<Vector2> autoMoveQueue = new Queue<Vector2>();

        #region "This Is undoStack Function"

        /// <summary>
        /// Saves the current player state (position) to the undo stack.
        /// </summary>
        public void SaveStateForUndo(Vector2 currentPosition)
        {
            // บันทึกตำแหน่งเฉพาะเมื่อแตกต่างจากตำแหน่งล่าสุดใน Stack
            if (undoStack.Count == 0 || undoStack.Peek() != currentPosition)
            {
                undoStack.Push(currentPosition);
                // ล้าง Redo Stack เมื่อผู้เล่นกระทำแอ็กชันใหม่
                redoStack.Clear();
            }
        }

        /// <summary>
        /// Reverts the player's state to the previous one using the undo stack.
        /// </summary>
        public void UndoLastMove(OOPPlayer player)
        {
            if (undoStack.Count > 0)
            {
                // เก็บตำแหน่งปัจจุบันลงใน Redo Stack ก่อนทำการ Undo
                Vector2 currentPos = player.transform.position;
                redoStack.Push(currentPos);

                // ดึงตำแหน่งก่อนหน้าออกจาก Undo Stack แล้วย้ายผู้เล่นกลับไป
                Vector2 previousPos = undoStack.Pop();
                player.transform.position = previousPos;
            }
            else
            {
                Debug.Log("No actions to undo.");
            }
        }

        public void RedoLastMove(OOPPlayer player)
        {
            if (redoStack.Count > 0)
            {
                // เก็บตำแหน่งปัจจุบันลงใน Undo Stack ก่อนทำการ Redo
                Vector2 currentPos = player.transform.position;
                undoStack.Push(currentPos);

                // ดึงตำแหน่งถัดไปออกจาก Redo Stack แล้วย้ายผู้เล่นไป
                Vector2 nextPos = redoStack.Pop();
                player.transform.position = nextPos;
            }
            else
            {
                Debug.Log("No actions to redo.");
            }
        }

        #endregion

        #region "This Is autoMoveQueue Function"

        public void StartAutoMoveSequence(OOPPlayer player)
        {
            // ล้างข้อมูลใน Queue ก่อนเริ่มสร้างลำดับการเดินใหม่
            autoMoveQueue.Clear();

            // 1. Prepare the Queue with the sequence of moves (ตัวอย่างการใส่จุดหมายการเดิน)
            Vector2 currentPos = player.transform.position;
            
            autoMoveQueue.Enqueue(currentPos + Vector2.up);
            autoMoveQueue.Enqueue(currentPos + Vector2.up + Vector2.right);
            autoMoveQueue.Enqueue(currentPos + Vector2.right);
            autoMoveQueue.Enqueue(currentPos);

            // Start the coroutine to process the auto-move sequence
            StartCoroutine(ProcessAutoMoveSequence(player));
        }

        public IEnumerator ProcessAutoMoveSequence(OOPPlayer player)
        {
            player.isAutoMoving = true;
            Debug.Log($"Auto-move sequence started with {autoMoveQueue.Count} steps.");

            yield return new WaitForSeconds(0.5f); // Initial delay before starting

            // 2. Process the Queue step-by-step
            while (autoMoveQueue.Count > 0)
            {
                // บันทึกตำแหน่งเดิมไว้ใน Undo Stack ก่อนจะย้ายตำแหน่งอัตโนมัติ
                SaveStateForUndo(player.transform.position);

                // ดึงตำแหน่งถัดมาจาก Queue ตามลำดับ FIFO
                Vector2 targetPos = autoMoveQueue.Dequeue();
                player.transform.position = targetPos;

                yield return new WaitForSeconds(0.5f); // หน่วงเวลาในแต่ละก้าว
            }

            player.isAutoMoving = false;
            Debug.Log("Auto-move sequence finished.");
        }

        #endregion
    }
}