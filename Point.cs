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
            return TryParse (c.message, out p);
        }
        public static bool TryParse(Osc.Message m, out Point p) {
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
            var x = (float)data [2];
            var y = (float)data [3];
            var w = (float)data [4];
            var h = (float)data [5];
            return new Point ((int)data [0], (int)data [1],
                new Rect (x - 0.5f * w, y - 0.5f * h, w, h),
                Time.timeSinceLevelLoad);            
        }

        public override string ToString () {
            return string.Format ("[Point:id={0},type={1},rect={2},time={3}]", id, type, rect, time);
        }
    }
}
