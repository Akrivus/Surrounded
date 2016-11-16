using System;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Surrounded.Source.Game.Render
{
    public class Directions
    {
        // Our directions.
        public static int Down = 0;
        public static int Left = 1;
        public static int Right = 2;
        public static int Up = 3;
        public static int Center = 4;

        // Gets a direction from a specific key.
        public static int GetDirectionFromKey(Keyboard.Key keyCode)
        {
            // Check what key was pressed.
            if (keyCode == Keyboard.Key.Down || keyCode == Keyboard.Key.S)
            {
                return Directions.Down;
            }
            else if (keyCode == Keyboard.Key.Left || keyCode == Keyboard.Key.A)
            {
                return Directions.Left;
            }
            else if (keyCode == Keyboard.Key.Right || keyCode == Keyboard.Key.D)
            {
                return Directions.Right;
            }
            else if (keyCode == Keyboard.Key.Up || keyCode == Keyboard.Key.W)
            {
                return Directions.Up;
            }

            // If it isn't in here return something else.
            return Directions.Center;
        }

        // Gets a vector from a specific direction.
        public static Vector3f GetVectorFromDirection(int direction)
        {
            if (direction == Directions.Down)
            {
                return new Vector3f(0, 1, 0);
            }
            else if (direction == Directions.Left)
            {
                return new Vector3f(-1, 0, 0);
            }
            else if (direction == Directions.Right)
            {
                return new Vector3f(1, 0, 0);
            }
            else if (direction == Directions.Up)
            {
                return new Vector3f(0, -1, 0);
            }
            return new Vector3f(0, 0, 0);
        }

        // Moves vector a specific direction.
        public static Vector2f MoveVectorInDirection(Vector2f[] corners, int direction, float speed)
        {
            // The position variables.
            Vector2f position = corners[4];

            // Determine the new position based on direction and movement.
            if (direction == Directions.Down)
            {
                return new Vector2f(position.X, position.Y + speed);
            }
            else if (direction == Directions.Left)
            {
                return new Vector2f(position.X - speed, position.Y);
            }
            else if (direction == Directions.Right)
            {
                return new Vector2f(position.X + speed, position.Y);
            }
            else if (direction == Directions.Up)
            {
                return new Vector2f(position.X, position.Y - speed);
            }
            return position;
        }
    }
}
