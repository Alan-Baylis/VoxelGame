using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Utils {
    public static class Logger {
        public static void Info(object message) => Log(ConsoleColor.White, "Info", message.ToString());
        public static void Debug(object message) => Log(ConsoleColor.Cyan, "Debug", message.ToString());
        public static void Warning(object message) => Log(ConsoleColor.Yellow, "Warning", message.ToString());
        public static void Error(object message) => Log(ConsoleColor.Red, "Error", message.ToString());

        public static void Exception(Exception ex) => Log(ConsoleColor.DarkRed, 
            "Exception", $"An unhandled exception has been thrown: {ex.Message}\r\nStackTrace: {ex.StackTrace}");

        private static void Log(ConsoleColor color, string level, string message) {
            message = message.Replace("\n", "\n" + new string(' ', level.Length + 3));

            QueuedConsole.Write($"[{level}] ", color);
            QueuedConsole.WriteLine(message);
        }
    }
}
