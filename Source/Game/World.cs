using System;

using SFML.Graphics;
using SFML.System;

namespace Surrounded.Source.Game
{
    public class World
    {
        // The layers that the game will draw to the screen.
        public Sprite UpperLayers;
        public Sprite LowerLayers;

        // The textures used to specify collision points.
        public Image CollisionMap;

        // The player's spawn point.
        public Vector2f SpawnPoint;

        // Class constructor.
        public World(string fileName)
        {

        }

        // Checks if a player can move to this specific point.
        public bool CanMoveTo(Vector2f position, bool noClip = false)
        {
            if ((position.X > this.CollisionMap.Size.X || position.X < 0) || (position.Y > this.CollisionMap.Size.Y || position.Y < 0))
            {
                return false;
            }
            else
            {
                if (noClip || this.CollisionMap.GetPixel(Convert.ToUInt32(position.X), Convert.ToUInt32(position.Y)).A < 255)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Calculates the player's speed on a specific point.
        public float CalculateSpeed(Vector2f position, float baseSpeed)
        {
            return baseSpeed * (1.0F - (this.CollisionMap.GetPixel(Convert.ToUInt32(position.X), Convert.ToUInt32(position.Y)).A / 255));
        }
    }
}
