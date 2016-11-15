using System;
using System.IO;

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
        public World()
        {
            this.UpperLayers = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "upper_layers.png")));
            this.LowerLayers = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "lower_layers.png")));
            this.CollisionMap = new Image(Path.Combine(Environment.CurrentDirectory, "files", "textures", "collisions.png"));
            this.SpawnPoint = new Vector2f(300, 300);
        }

        // Checks if a player can move to this specific point.
        public bool CanMoveTo(Vector2f[] corners, bool noClip = false)
        {
            int cornersMatched = 0;
            for (int corner = 0; corner < corners.Length; ++corner)
            {
                if ((corners[corner].X >= this.CollisionMap.Size.X || corners[corner].X <= 0) || (corners[corner].Y >= this.CollisionMap.Size.Y || corners[corner].Y <= 0))
                {
                    return false;
                }
                else
                {
                    if (noClip || this.CollisionMap.GetPixel(Convert.ToUInt32(corners[corner].X), Convert.ToUInt32(corners[corner].Y)).A < 255)
                    {
                        ++cornersMatched;
                    }
                }
            }
            return cornersMatched == 4;
        }
    }
}
