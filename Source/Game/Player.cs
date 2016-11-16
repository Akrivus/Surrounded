using Microsoft.Win32.SafeHandles;

using System;
using System.IO;
using System.Runtime.InteropServices;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Surrounded.Source.Game.Render;
using System.Collections.Generic;

namespace Surrounded.Source.Game
{
    public class Player : IDisposable
    {
        // This is used to get the input keys.
        public static Keyboard.Key[] MovementKeys = new Keyboard.Key[]
        {
            Keyboard.Key.Down,
            Keyboard.Key.Left,
            Keyboard.Key.Right,
            Keyboard.Key.Up,
            Keyboard.Key.S,
            Keyboard.Key.A,
            Keyboard.Key.D,
            Keyboard.Key.W
        };
        public static Keyboard.Key[] ControlKeys = new Keyboard.Key[]
        {
            Keyboard.Key.RControl,
            Keyboard.Key.LControl,
            Keyboard.Key.Q
        };
        private List<int> LastDirections = new List<int>();

        // These are system things used for memory management.
        private SafeHandle Handle = new SafeFileHandle(IntPtr.Zero, true);
        private bool Disposed = false;

        // This is used to attach the player to a map object.
        private Map CurrentMap;

        // These are used explicitly for rendering.
        private AnimatedSprite Sprite;
        private Vector2f Position;
        private bool Walking;
        private bool Attacking;
        private bool Dead;

        // These are used as the player's stats.
        private float Health = 10.0F;
        private float Stamina = 5.0F;
        private float Stress = 0.0F;
        private float Speed = 2.0F;

        // Class constructor.
        public Player()
        {
            // Create the player sprite.
            this.Sprite = new AnimatedSprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "player.png")), 120);
        }

        // Attaches the player to a map.
        public void AttachToMap(Map newMap)
        {
            this.CurrentMap = newMap;
            this.Position = this.CurrentMap.SpawnPoint;
        }

        // Handles movement.
        private void HandleMovement()
        {
            for (int i = 0; i < Player.MovementKeys.Length; ++i)
            {
                int Direction = Directions.GetDirectionFromKey(Player.MovementKeys[i]);
                if (Keyboard.IsKeyPressed(Player.MovementKeys[i]) && !LastDirections.Contains(Direction))
                {
                    LastDirections.Add(Direction);
                }
                else if (LastDirections.Contains(Direction))
                {
                    LastDirections.Remove(Direction);
                }
            }

            if (LastDirections.Count > 0)
            {
                int LastDirection = LastDirections[LastDirections.Count - 1];
                this.Sprite.Direction = LastDirection;
                Vector2f newPosition = Directions.MoveVectorInDirection(this.Sprite.GetCorners(this.Position), LastDirection, this.Speed);
                //this.Attacking = false;

                // Did we really walk though?
                if (this.CurrentMap.CanMoveTo(this.Sprite.GetCorners(newPosition)))
                {
                    this.Walking = true;
                    this.Position = newPosition;
                }
                else
                {
                    this.Walking = false;
                }
            }
            else
            {
                this.Walking = false;
            }
        }

        // Handles attack keys.
        private void HandleControl()
        {
            // Get the direct key input.
            for (int i = 0; i < Player.ControlKeys.Length; ++i)
            {
                if (Keyboard.IsKeyPressed(Player.ControlKeys[i]))
                {
                    this.Attacking = true;
                    this.Walking = false;
                    break;
                }
            }
        }

        // Checks if player is attacking.
        private void HandleStatus()
        {
            if (this.Health < 1)
            {
                this.Dead = true;
            }
            if (this.Attacking && this.Sprite.Step != 3)
            {
                this.Attacking = false;
            }
        }

        // Called when the player needs to be updated.
        public void Update(Surrounded game)
        {
            // Handles input.
            HandleMovement();
            HandleControl();
            HandleStatus();

            // Update listener direction and position.
            Listener.Direction = Directions.GetVectorFromDirection(this.Sprite.Direction);
            Listener.Position = new Vector3f(this.Position.X, this.Position.Y, 0);

            // Update the sprite.
            this.Sprite.Update(this.Position, this.Walking, this.Attacking, this.Dead);

            // Update the camera position.
            game.SetView(new View(this.Position, new Vector2f(640, 360)));

            // Add the aura.
            game.Lights.Add(new Light(this.Position, Color.Yellow, 1.0F));
        }

        // Provides instructions on drawing the player.
        public void Draw(RenderWindow surface, RenderTexture foreground)
        {
            this.Sprite.Draw(surface, RenderStates.Default);
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
                this.CurrentMap.Dispose();
                this.Sprite.Dispose();

                // Finish with the object itself.
                Handle.Dispose();
            }
            // We're disposed now.
            this.Disposed = true;
        }
    }
}
