using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IwVoxelGame.Utils {
    public static class QueuedConsole {
        private static BlockingCollection<Message> m_Queue = new BlockingCollection<Message>();

        static QueuedConsole() {
            Thread thread = new Thread(() => {
                while (true) {
                    if (m_Queue.Count > 0) {
                        Message message = m_Queue.Take();

                        Console.ForegroundColor = message.color;
                        Console.Write(message.value);
                    } else {
                        Thread.Sleep(10);
                    }
                }
              });

            thread.IsBackground = true;
            thread.Start();
        }

        public static void WriteLine(string value) {
            m_Queue.Add(new Message(value + "\r\n", ConsoleColor.Gray));
        }

        public static void Write(string value) {
            m_Queue.Add(new Message(value, ConsoleColor.Gray));
        }

        public static void WriteLine(string value, ConsoleColor color) {
            m_Queue.Add(new Message(value + "\r\n", color));
        }

        public static void Write(string value, ConsoleColor color) {
            m_Queue.Add(new Message(value, color));
        }

        private struct Message {
            public string value;
            public ConsoleColor color;

            public Message(string value, ConsoleColor color) {
                this.value = value;
                this.color = color;
            }
        }
    }
}
