using System;
using System.IO;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Surrounded.Source.Game
{
    public class Player
    {
        // This is used to attach the player to a map object.
        private Map CurrentMap;

        // These are used explicitly for rendering.
        private Sprite Sprite;
        private Vector2f Position;
        private Clock FrameTimer;
        private bool Walking;
        private int Direction;
        private int Step;

        // These are used as the player's stats.
        private float Health = 10.0F;
        private float Stamina = 5.0F;
        private float Stress = 0.0F;
        private float Speed = 3.5F;

        // Class constructor.
        public Player()
        {
            // Create the player sprite.
            this.Sprite = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "player.png")));
            this.Sprite.Origin = new Vector2f(16, 24);

            // Create the player's frame timer.
            this.FrameTimer = new Clock();
        }

        // Attaches the player to a map.
        public void AttachToMap(Map newMap)
        {
            this.CurrentMap = newMap;
            this.Position = this.CurrentMap.SpawnPoint;
        }

        // Moves the player's position by direction, provided a valid movement.
        public void Move()
        {
            Vector2f newPosition;
            if (this.Direction == 0) { newPosition = new Vector2f(this.Position.X, this.Position.Y + this.Speed); }
            else if (this.Direction == 1) { newPosition = new Vector2f(this.Position.X - this.Speed, this.Position.Y); }
            else if (this.Direction == 2) { newPosition = new Vector2f(this.Position.X + this.Speed, this.Position.Y); }
            else if (this.Direction == 3) { newPosition = new Vector2f(this.Position.X, this.Position.Y - this.Speed); }
            else { return; }

            if (this.CurrentMap.CanMoveTo(this.GetCorners(newPosition, 32, 48))) { this.Position = newPosition; }
        }

        // Called when the player presses a key.
        public void OnKeyPressed(Keyboard.Key keyCode, bool shiftDown)
        {
            // Determine direction.
            if (keyCode == Keyboard.Key.Down) { this.Direction = 0; }
            else if (keyCode == Keyboard.Key.Left) { this.Direction = 1; }
            else if (keyCode == Keyboard.Key.Right) { this.Direction = 2; }
            else if (keyCode == Keyboard.Key.Up) { this.Direction = 3; }
            else { return; }

            // Will only be called if one of the first four keys (directional) are pressed.
            this.Walking = true;
        }

        // Called when the player releases a key.
        public void OnKeyReleased(Keyboard.Key keyCode, bool shiftDown)
        {
            // Determine direction.
            if (keyCode == Keyboard.Key.Down) { this.Direction = 0; }
            else if (keyCode == Keyboard.Key.Left) { this.Direction = 1; }
            else if (keyCode == Keyboard.Key.Right) { this.Direction = 2; }
            else if (keyCode == Keyboard.Key.Up) { this.Direction = 3; }
            else { return; }

            // If they stopped walking, go into a standing position.
            // Will only be called if one of the first four keys (directional) are pressed.
            this.Walking = false;
            this.Step = 0;
        }

        // Called when the player needs to be updated.
        public void Update(Surrounded game)
        {
            // Update the sprite's frame if the sprite is walking.
            if (this.Walking)
            {
                this.Move();
                // Change the frame every 0.3 seconds, adjust if necessary.
                if (this.FrameTimer.ElapsedTime.AsMilliseconds() > 300)
                {
                    ++this.Step;
                    if (this.Step > 2)
                    {
                        this.Step = 1;
                    }
                    this.FrameTimer.Restart();
                }
            }
            else
            {
                // If we aren't walking, then stand.
                Step = 0;
            }

            // If the player turned, change the direction of the listener.
            if (this.Direction == 0) // Down.
            {
                Listener.Direction = new Vector3f(0, 1, 0);
            }
            else if (this.Direction == 1) // Left.
            {
                Listener.Direction = new Vector3f(-1, 0, 0);
            }
            else if (this.Direction == 2) // Right.
            {
                Listener.Direction = new Vector3f(1, 0, 0);
            }
            else if (this.Direction == 3) // Up.
            {
                Listener.Direction = new Vector3f(0, -1, 0);
            }

            // Update positions.
            Listener.Position = new Vector3f(this.Position.X, this.Position.Y, 0);
            game.SetView(new View(this.Position, new Vector2f(640, 360)));
            this.Sprite.Position = this.Position;

            // Add the aura.
            game.Lights.Add(new Light(this.Position, new Color(Convert.ToByte(Surrounded.RNG.Next(255)), Convert.ToByte(Surrounded.RNG.Next(255)), Convert.ToByte(Surrounded.RNG.Next(255))), 1.0F));

            // Update the sprite's texture.
            this.Sprite.TextureRect = new IntRect(Step * 32, Direction * 48, 32, 48);
        }

        // Gets the sprite's corners and origin.
        public Vector2f[] GetCorners(Vector2f position, float width, float height)
        {
            // Return all four corners and the origin.
            return new Vector2f[] {
                new Vector2f(position.X - (width / 2), position.Y - (height / 2)),
                new Vector2f(position.X - (width / 2), position.Y + (height / 2)),
                new Vector2f(position.X + (width / 2), position.Y - (height / 2)),
                new Vector2f(position.X + (width / 2), position.Y + (height / 2)),
                new Vector2f(position.X, position.Y)
            };
        }

        // Provides instructions on drawing the player.
        public void Draw(RenderWindow surface, RenderTexture foreground)
        {
            this.Sprite.Draw(surface, RenderStates.Default);
        }
    }
}
