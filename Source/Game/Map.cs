using System;
using System.Collections.Generic;
using System.IO;

using SFML.Graphics;
using SFML.System;

namespace Surrounded.Source.Game
{
    public class Map
    {
        // These are the visual parts of the map that we see.
        private Sprite UpperLayers;
        private Sprite LowerLayers;

        // These are players and other drawable objects that are bound to change.
        private Player Player;
        private List<Light> Lights;

        // This is the set of pixels used to determine where something can and can't move.
        private Image CollisionMap;

        // This is the player's spawnpoint.
        public Vector2f SpawnPoint;

        // Class constructor.
        public Map(Player player)
        {
            this.UpperLayers = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "upper_layers.png")));
            this.LowerLayers = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "lower_layers.png")));
            this.CollisionMap = new Image(Path.Combine(Environment.CurrentDirectory, "files", "textures", "collisions.png"));

            this.SpawnPoint = new Vector2f(300, 300);
            this.Lights = new List<Light>();

            this.Player = player;
            this.Player.AttachToMap(this);
        }

        // Adds a light.
        public void AddLight(Light light)
        {
            this.Lights.Add(light);
        } 

        // Checks if something can move to a specific point.
        public bool CanMoveTo(Vector2f[] corners, bool noClip = false)
        {
            // Checks the corners of the object's hitbox for collisions.
            int cornersMatched = 0;
            for (int corner = 0; corner < corners.Length; ++corner)
            {
                if ((corners[corner].X >= this.CollisionMap.Size.X || corners[corner].X <= 0) || (corners[corner].Y >= this.CollisionMap.Size.Y || corners[corner].Y <= 0))
                {
                    return false;
                }
                else
                {
                    // No-clip was added for entities that can walk through walls (ex: bats, insects)
                    if (noClip || this.CollisionMap.GetPixel(Convert.ToUInt32(corners[corner].X), Convert.ToUInt32(corners[corner].Y)).A < 255)
                    {
                        ++cornersMatched;
                    }
                }
            }
            return cornersMatched == 5;
        }

        // Provides instructions on updating the map.
        public void Update(Surrounded game)
        {
            // Add lights.
            for (int i = 0; i < Lights.Count; ++i)
            {
                game.Lights.Add(this.Lights[i]);
            }

            // Update player logic.
            this.Player.Update(game);
        }

        // Provides instructions on drawing the map.
        public void Draw(RenderWindow surface, RenderTexture foreground)
        {
            // Draw the layer that will go underneath the player.
            this.LowerLayers.Draw(surface, RenderStates.Default);

            // Draw the player.
            this.Player.Draw(surface, foreground);

            // Draw the layer that will go above the player.
            this.UpperLayers.Draw(surface, RenderStates.Default);
        }
    }
}
