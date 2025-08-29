//==================================================================
//Author: Dave Hardin, Upperbay Systems LLC
//Author URL: https://upperbay.com
//License: MIT
//Date: 2001-2024
//Description: 
//Notes:
//==================================================================
using System;
using System.Collections.Generic;

namespace ChatterBoxGPT
{
    public class FIFOQueue<T>
    {
        private Queue<T> queue = new Queue<T>();

        // Enqueue (Write) an item to the end of the queue
        public void Enqueue(T item)
        {
            queue.Enqueue(item);
        }

        // Dequeue (Read) an item from the front of the queue
        public T Dequeue()
        {
            if (queue.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }
            return queue.Dequeue();
        }

        // Peek (Read) the item at the front of the queue without removing it
        public T Peek()
        {
            if (queue.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }
            return queue.Peek();
        }

        // Check if the queue is empty
        public bool IsEmpty()
        {
            return queue.Count == 0;
        }

        // Get the number of items in the queue
        public int Count()
        {
            return queue.Count;
        }

        // Clear all items from the queue
        public void Clear()
        {
            queue.Clear();
        }
    }

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        // Create a FIFOQueue of integers
    //        FIFOQueue<string> fifoQueue = new FIFOQueue<string>();

    //        // Enqueue some items
    //        fifoQueue.Enqueue("1");
    //        fifoQueue.Enqueue("2");
    //        fifoQueue.Enqueue("3");

    //        // Dequeue items and print them
    //        while (!fifoQueue.IsEmpty())
    //        {
    //            string item = fifoQueue.Dequeue();
    //            Console.WriteLine($"Dequeued: {item}");
    //        }

    //        // Check if the queue is empty
    //        Console.WriteLine($"Is Empty: {fifoQueue.IsEmpty()}");
    //    }
    //}

}
