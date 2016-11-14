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
        // World related variables.
        public World CurrentWorld;

        // Rendering related variables.
        public Sprite Sprite;
        public Sprite Light;

        // Movement related variables.
        public Vector2f Position;
        public bool Walking;
        public int Speed;

        // Frame related variables.
        public Clock FrameTimer;
        public int Direction;
        public int Step;

        // Class constructor.
        public Player(World currentWorld)
        {
            // Create the player sprite.
            this.Sprite = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "player.png")));
            this.Sprite.Origin = new Vector2f(16, 24);

            // Create the player's light.
            this.Light = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "player_light.png")));
            this.Light.Origin = new Vector2f(64, 64);

            // Create the player's world and positioning.
            this.CurrentWorld = currentWorld;
            this.Position = this.CurrentWorld.SpawnPoint;

            // Create the player's frame timer.
            this.FrameTimer = new Clock();
        }

        // Called when the player presses a key.
        public void OnKeyPressed(Keyboard.Key keyCode, bool shiftDown)
        {
            // Check if they were trying to walk.
            bool wasWalking = this.Walking;
            if (keyCode == Keyboard.Key.Down)
            {
                this.Direction = 0;
                this.Position = new Vector2f(this.Position.X, this.Position.Y + 3);
                this.Walking = true;
            }
            else if (keyCode == Keyboard.Key.Left)
            {
                this.Direction = 1;
                this.Position = new Vector2f(this.Position.X - 3, this.Position.Y);
                this.Walking = true;
            }
            else if (keyCode == Keyboard.Key.Right)
            {
                this.Direction = 2;
                this.Position = new Vector2f(this.Position.X + 3, this.Position.Y);
                this.Walking = true;
            }
            else if (keyCode == Keyboard.Key.Up)
            {
                this.Direction = 3;
                this.Position = new Vector2f(this.Position.X, this.Position.Y - 3);
                this.Walking = true;
            }

            // If they just started walking, reset the frame timer to prevent a gliding effect.
            if (this.Walking && !wasWalking)
            {
                this.FrameTimer.Restart();
            }
        }

        // Called when the player releases a key.
        public void OnKeyReleased(Keyboard.Key keyCode, bool shiftDown)
        {
            // Check if they were trying to stop walking.
            if (keyCode == Keyboard.Key.Down)
            {
                this.Direction = 0;
                this.Walking = false;
            }
            else if (keyCode == Keyboard.Key.Left)
            {
                this.Direction = 1;
                this.Walking = false;
            }
            else if (keyCode == Keyboard.Key.Right)
            {
                this.Direction = 2;
                this.Walking = false;
            }
            else if (keyCode == Keyboard.Key.Up)
            {
                this.Direction = 3;
                this.Walking = false;
            }

            // If they stopped walking, 
            if (!this.Walking)
            {
                this.Step = 0;
            }
        }

        // Called when the player needs to be updated.
        public void Update(Surrounded game)
        {
            // Update the sprite's frame.
            if (this.Walking)
            {
                if (this.FrameTimer.ElapsedTime.AsMilliseconds() > 300)
                {
                    this.Step += 1;
                    if (this.Step > 2)
                    {
                        this.Step = 1;
                    }
                    this.FrameTimer.Restart();
                }
            }
            else
            {
                Step = 0;
            }

            // Update direction.
            if (this.Direction == 0)
            {
                Listener.Direction = new Vector3f(0, -1, 0);
            }
            else if (this.Direction == 1)
            {
                Listener.Direction = new Vector3f(1, 0, 0);
            }
            else if (this.Direction == 2)
            {
                Listener.Direction = new Vector3f(-1, 0, 0);
            }
            else if (this.Direction == 3)
            {
                Listener.Direction = new Vector3f(0, 1, 0);
            }

            // Update position.
            Listener.Position = new Vector3f(this.Position.X, this.Position.Y, 0);

            // Add a light.
            this.Light.Position = this.Position;
            game.Lights.Add(this.Light);

            // Update the sprite's texture.
            this.Sprite.Position = this.Position;
            this.Sprite.TextureRect = new IntRect(Step * 32, Direction * 48, 32, 48);
        }
    }
}
