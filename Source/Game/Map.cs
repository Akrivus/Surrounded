using Microsoft.Win32.SafeHandles;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using SFML.Graphics;
using SFML.System;

using Newtonsoft.Json;

using Surrounded.Source.Game.Entities;

namespace Surrounded.Source.Game
{
    public class Map : IDisposable
    {
        // These are system things used for memory management.
        private SafeHandle Handle = new SafeFileHandle(IntPtr.Zero, true);
        private bool Disposed = false;

        // These are the visual parts of the map that we see.
        private Sprite UpperLayers;
        private Sprite LowerLayers;

        // These are players and other drawable objects that are bound to change.
        private List<Light> Lights;
        private List<Entity> Entities;
        private Player Player;

        // This is the set of pixels used to determine where something can and can't move.
        private Image CollisionMap;

        // This is the player's spawnpoint.
        public Vector2f SpawnPoint;
        public Text Name;

        // Class constructor.
        public Map(Player player, string fileName)
        {
            // Loads the map from the computer.
            this.UpperLayers = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "maps", fileName, "upper_layers.png")));
            this.LowerLayers = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "maps", fileName, "lower_layers.png")));
            this.CollisionMap = new Image(Path.Combine(Environment.CurrentDirectory, "files", "maps", fileName, "collisions.png"));

            // Create lists.
            this.Lights = new List<Light>();
            this.Entities = new List<Entity>();

            // Loads lights from JSON.
            StreamReader fileReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, "files", "maps", fileName, "lights.json"));
            JSON.Light[] lights = JsonConvert.DeserializeObject<JSON.Light[]>(fileReader.ReadToEnd());
            fileReader.Close();

            // Loads the lights.
            for (int i = 0; i < lights.Length; ++i)
            {
                this.Lights.Add(Light.ConvertFromJson(lights[i]));
            }

            // Loads lights from JSON.
            fileReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, "files", "maps", fileName, "entities.json"));
            JSON.Entity[] entities = JsonConvert.DeserializeObject<JSON.Entity[]>(fileReader.ReadToEnd());
            fileReader.Close();

            // Loads the lights.
            for (int i = 0; i < entities.Length; ++i)
            {
                this.Entities.Add(Entity.ConvertFromJson(entities[i]));
            }

            // Loads spawnpoint from JSON.
            fileReader = File.OpenText(Path.Combine(Environment.CurrentDirectory, "files", "maps", fileName, "spawn_point.json"));
            this.SpawnPoint = JsonConvert.DeserializeObject<Vector2f>(fileReader.ReadToEnd());
            fileReader.Dispose();

            // Loads the player.
            this.Player = player;
            this.Player.AttachToMap(this);

            // Creates a text object.
            this.Name = new Text("map name: " + fileName, new Font(Path.Combine(Environment.CurrentDirectory, "files", "sansation.ttf")), 12);
            this.Name.Color = Color.Yellow;
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

            // Update map name position.
            this.Name.Position = new Vector2f(game.GetView().Center.X - (game.GetView().Size.X / 2), game.GetView().Center.Y - (game.GetView().Size.Y / 2));

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

        // Disposes the object.
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            // If this is already disposed, then ignore.
            if (this.Disposed)
            {
                return;
            }
            else if (disposing)
            {
                // Dispose all other disposable objects here.
                this.UpperLayers.Dispose();
                this.LowerLayers.Dispose();
                this.Player.Dispose();

                // Dispose all of the lights.
                for (int i = 0; i < Lights.Count; ++i)
                {
                    this.Lights[i].Dispose();
                }

                // Continue disposing all the disposables.
                this.CollisionMap.Dispose();

                // Finish with the object itself.
                Handle.Dispose();
            }
            // We're disposed now.
            this.Disposed = true;
        }
    }
}
