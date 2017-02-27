using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PointRegistrationSubmod {

    public struct Point {
        public const string PATH = "/object";

        public readonly int id;
        public readonly int type; // 0:hill, 1:lake, 2:block
        public readonly Rect rect;
        public readonly float time;

        public Point(int id, int type, Rect rect, float time) {
            this.id = id;
            this.type = type;
            this.rect = rect;
            this.time = time;
        }

        public static bool TryParse(Osc.OscPort.Capsule c, out Point p) {
            var m = c.message;
            try {
                if (m.path == PATH) {
                    p = Parse(m.data);
                    return true;
                }
            } catch (System.Exception){
            }

            p = default(Point);
            return false;
        }
        public static Point Parse(object[] data) {
            return new Point ((int)data [0], (int)data [1],
                new Rect ((float)data [2], (float)data [3], (float)data [4], (float)data [5]),
                Time.timeSinceLevelLoad);            
        }

        public override string ToString () {
            return string.Format ("[Point:id={0},type={1},rect={2},time={3}]", id, type, rect, time);
        }
    }
}
