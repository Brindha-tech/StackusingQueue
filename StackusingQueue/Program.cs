using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace StackusingQueue
{
    class Program
    {
        //public class StackImplementation
        //{

        //IMPLEMENT STACK USING QUEUE
        //Queue<int> Que = new Queue<int>();

        //public void Push(int value)
        //{
        //    Que.Enqueue(value);
        //    Console.WriteLine("Pushed {0}", value);
        //}

        //public int pop()
        //{
        //    if (Que.Count == 0)
        //    {
        //        Console.WriteLine("Stack is empty");
        //        return -1;
        //    }
        //    else
        //    {
        //        int counter = Que.Count;
        //        while (counter > 1)
        //        {
        //            Que.Enqueue(Que.Dequeue());
        //            counter--;
        //        }                    
        //        return Que.Dequeue();
        //    }
        //}

        //IMPLEMENT QUEUE USING STACK
        //Stack<int> stk = new Stack<int>();
        //int[] arr = new int[30];

        //public void Enqueue(int value) {
        //    stk.Push(value);
        //    Console.WriteLine("Enqueued {0}", value);
        //}

        //public int Dequeue()
        //{
        //    if (stk.Count == 0)
        //        return -1;
        //    else
        //    {
        //        int counter = stk.Count - 1;
        //        int c1 = 0;
        //        for (int i=0; i<counter; i++)
        //        {
        //            arr[i] = stk.Pop();
        //            c1++;
        //        }
        //        int poppedValue = stk.Pop();
        //        for(int j = c1-1; j>=0; j--)
        //        {
        //            stk.Push(arr[j]);
        //        }
        //        return poppedValue;
        //    }
        //}



        //}
        //static void Main(string[] args)
        //{
        //    StackImplementation s = new StackImplementation();
        //    s.Enqueue(5);
        //    s.Enqueue(4);
        //    s.Enqueue(3);
        //    s.Enqueue(2);
        //    s.Enqueue(1);
        //    int i = s.Dequeue();
        //    Console.WriteLine("Dequeuing {0}", i);
        //    int j = s.Dequeue();
        //    Console.WriteLine("Dequeuing {0}", j);
        //    Console.ReadKey();
        //}

        int i = 0;
        static void Main2(string[] args)
        {

            //Thread t = new Thread(childFn);
            //t.Start();

            //t.Join();
            ThreadSample sample = new ThreadSample();
            Thread producer = new Thread(sample.Producer);
            //Thread producer = new Thread(() =>
            //   {
            //       while (true)
            //       {
            //           //Thread.Sleep(1000);
            //           sample.Producer();
            //       }
            //   });

            Thread consumer = new Thread(() =>
            {
                while (true)
                {
                    //Thread.Sleep(1000);
                    sample.Consumer();
                }
            });

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();
            Console.WriteLine("Hi Everyone");
            Console.ReadLine();
        }

        static void childFn()
        {

            Console.WriteLine("child start");
            Thread.Sleep(100);


            Console.WriteLine("child end");
        }


        public class ThreadSample
        {

            int current = 0;
            static int size=10;
            private readonly object _lock = new object();
            static Mutex mutex = new Mutex();
            public void Producer()
            {                
                lock (_lock)
                {
                    while (current == size)
                    {
                        Console.WriteLine("Producer waiting");
                        Monitor.Wait(_lock);
                    } 
                    current++;
                    Console.WriteLine("Producer {0}", current);
                    Monitor.Pulse(_lock);
                }
            }
            public void Consumer()
            {
                lock (_lock) 
                {
                    while(current == 0)
                    {
                        Console.WriteLine("Consumer waiting");

                        Monitor.Wait(_lock);
                    }
                    Console.WriteLine("Consumer took {0}", current);
                    current--;
                    Monitor.Pulse(_lock);
                }
            }

        }
    }       

}
