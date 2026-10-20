using System;
using System.Collections.Generic;

namespace Assignment
{
    public class StudentSolution
    {
        class Action
        {
            public string Name;
            public int Value;
        }

        #region Lecture

        public (int Count, int Popped, int Peek, int CountAfterPeek) LCT01_StackSyntax()
        {
            Stack<int> stack = new Stack<int>();

            // Push ข้อมูลเข้า Stack (LIFO: Last In, First Out)
            stack.Push(10);
            stack.Push(20);
            stack.Push(30);

            int count = stack.Count;           // 3
            int popped = stack.Pop();          // 30 (ดึงตัวล่าสุดออก)
            int peek = stack.Peek();           // 20 (แอบดูตัวบนสุดโดยไม่ดึงออก)
            int countAfterPeek = stack.Count;  // 2 (จำนวนคงเดิมหลัง Peek)

            return (count, popped, peek, countAfterPeek);
        }

        public (int Count, int Dequeued, int Peek, int CountAfterPeek) LCT02_QueueSyntax()
        {
            Queue<int> queue = new Queue<int>();

            // Enqueue ข้อมูลเข้า Queue (FIFO: First In, First Out)
            queue.Enqueue(10);
            queue.Enqueue(20);
            queue.Enqueue(30);

            int count = queue.Count;             // 3
            int dequeued = queue.Dequeue();      // 10 (ดึงตัวแรกสุดออก)
            int peek = queue.Peek();             // 20 (แอบดูตัวแรกสุดโดยไม่ดึงออก)
            int countAfterPeek = queue.Count;    // 2 (จำนวนคงเดิมหลัง Peek)

            return (count, dequeued, peek, countAfterPeek);
        }

        public string[] LCT03_ActionStack()
        {
            Action action1 = new Action { Name = "Action 1" };
            Action action2 = new Action { Name = "Action 2" };
            Action action3 = new Action { Name = "Action 3" };

            Stack<Action> stack = new Stack<Action>();
            stack.Push(action1);
            stack.Push(action2);
            stack.Push(action3);

            // Pop ออกมาประมวลผล จะได้ลำดับย้อนกลับ (LIFO): Action 3 -> Action 2 -> Action 1
            List<string> result = new List<string>();
            while (stack.Count > 0)
            {
                result.Add(stack.Pop().Name);
            }

            return result.ToArray();
        }

        public string[] LCT04_ActionQueue()
        {
            Action action1 = new Action { Name = "Action 1" };
            Action action2 = new Action { Name = "Action 2" };
            Action action3 = new Action { Name = "Action 3" };

            Queue<Action> queue = new Queue<Action>();
            queue.Enqueue(action1);
            queue.Enqueue(action2);
            queue.Enqueue(action3);

            // Dequeue ออกมาประมวลผล จะได้ลำดับตามจริง (FIFO): Action 1 -> Action 2 -> Action 3
            List<string> result = new List<string>();
            while (queue.Count > 0)
            {
                result.Add(queue.Dequeue().Name);
            }

            return result.ToArray();
        }

        #endregion

        #region Assignment

        public string ASN01_ReverseString(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            Stack<char> stack = new Stack<char>();

            // Push ตัวอักษรทุกตัวลงใน Stack
            foreach (char c in str)
            {
                stack.Push(c);
            }

            // Pop ตัวอักษรออกมาเรียงใหม่ ข้อความจะกลับหลังหันโดยอัตโนมัติ
            char[] reversedArr = new char[str.Length];
            int index = 0;
            while (stack.Count > 0)
            {
                reversedArr[index++] = stack.Pop();
            }

            return new string(reversedArr);
        }

        public bool ASN02_StackPalindrome(string str)
        {
            if (str == null) return false;

            Stack<char> stack = new Stack<char>();

            // Push ตัวอักษรทั้งหมดลงใน Stack
            foreach (char c in str)
            {
                stack.Push(c);
            }

            // นำตัวอักษรย้อนกลับจาก Stack มาเทียบกับตัวอักษรเดิมจากหน้าไปหลัง
            foreach (char c in str)
            {
                if (c != stack.Pop())
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Extra

        public bool EX01_ParenthesesChecker(string str)
        {
            if (str == null) return false;

            Stack<char> stack = new Stack<char>();

            foreach (char c in str)
            {
                // หากเจอวงเล็บเปิด ให้เก็บลงใน Stack
                if (c == '(' || c == '[' || c == '{')
                {
                    stack.Push(c);
                }
                // หากเจอวงเล็บปิด ให้ดึงตัวล่าสุดออกจาก Stack มาจับคู่
                else if (c == ')' || c == ']' || c == '}')
                {
                    if (stack.Count == 0) return false; // เจอวงเล็บปิดโดยไม่มีวงเล็บเปิดก่อนหน้า

                    char open = stack.Pop();
                    if ((c == ')' && open != '(') ||
                        (c == ']' && open != '[') ||
                        (c == '}' && open != '{'))
                    {
                        return false; // ชนิดของวงเล็บไม่สอดคล้องกัน
                    }
                }
            }

            // ถ้าสแต็กว่างแสดงว่าวงเล็บจับคู่ครบถูกต้องทั้งหมด
            return stack.Count == 0;
        }

        #endregion
    }
}
