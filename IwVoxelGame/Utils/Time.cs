using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IwVoxelGame.Utils {
    public static class Time {
        private static readonly DateTime _gameStart;

        /// <summary>
        /// Total milliseconds since the game started.
        /// </summary>
        public static double GameTime => (DateTime.UtcNow - _gameStart).TotalMilliseconds;

        static Time() {
            _gameStart = DateTime.UtcNow;
        }
    }
}
