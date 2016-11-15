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

        // Called when the player presses a key.
        public void OnKeyPressed(Keyboard.Key keyCode, bool shiftDown)
        {
            // Check if they were walking.
            bool wasWalking = this.Walking;

            // Check what key was pressed.
            if (keyCode == Keyboard.Key.Down)
            {
                // Create a new position and check if they are capable of moving to it.
                Vector2f newPosition = new Vector2f(this.Position.X, this.Position.Y + this.Speed);
                if (this.CurrentMap.CanMoveTo(this.GetCorners(newPosition, 32, 48)))
                {
                    this.Position = newPosition;
                    this.Walking = true;
                }
                else
                {
                    this.Walking = false;
                }
                this.Direction = 0; // Down.
            }
            else if (keyCode == Keyboard.Key.Left)
            {
                // Create a new position and check if they are capable of moving to it.
                Vector2f newPosition = new Vector2f(this.Position.X - this.Speed, this.Position.Y);
                if (this.CurrentMap.CanMoveTo(this.GetCorners(newPosition, 32, 48)))
                {
                    this.Position = newPosition;
                    this.Walking = true;
                }
                else
                {
                    this.Walking = false;
                }
                this.Direction = 1; // Left.
            }
            else if (keyCode == Keyboard.Key.Right)
            {
                // Create a new position and check if they are capable of moving to it.
                Vector2f newPosition = new Vector2f(this.Position.X + this.Speed, this.Position.Y);
                if (this.CurrentMap.CanMoveTo(this.GetCorners(newPosition, 32, 48)))
                {
                    this.Position = newPosition;
                    this.Walking = true;
                }
                else
                {
                    this.Walking = false;
                }
                this.Direction = 2; // Right.
            }
            else if (keyCode == Keyboard.Key.Up)
            {
                // Create a new position and check if they are capable of moving to it.
                Vector2f newPosition = new Vector2f(this.Position.X, this.Position.Y - this.Speed);
                if (this.CurrentMap.CanMoveTo(this.GetCorners(newPosition, 32, 48)))
                {
                    this.Position = newPosition;
                    this.Walking = true;
                }
                else
                {
                    this.Walking = false;
                }
                this.Direction = 3; // Up.
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
            // Check what key was pressed.
            if (keyCode == Keyboard.Key.Down)
            {
                this.Direction = 0; // Down.
                this.Walking = false;
            }
            else if (keyCode == Keyboard.Key.Left)
            {
                this.Direction = 1; // Left.
                this.Walking = false;
            }
            else if (keyCode == Keyboard.Key.Right)
            {
                this.Direction = 2; // Right.
                this.Walking = false;
            }
            else if (keyCode == Keyboard.Key.Up)
            {
                this.Direction = 3; // Up.
                this.Walking = false;
            }

            // If they stopped walking, go into a standing position.
            if (!this.Walking)
            {
                this.Step = 0;
            }
        }

        // Called when the player needs to be updated.
        public void Update(Surrounded game)
        {
            // Update the sprite's frame if the sprite is walking.
            if (this.Walking)
            {
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
