using System;
using System.Collections.Generic;
using System.IO;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Steamworks;

using Surrounded.Source.Game;

namespace Surrounded.Source
{
    public class Surrounded : RenderWindow
    {
        // Global variables accessible by the entire program.
        public static Options Options;
        public static Image Icon;
        public static Random RNG;
        public static bool SteamOnline;

        // The game instance itself.
        public static Surrounded Instance;

        // The player and other non-rendering related game elements.
        public Player Player;
        public World World;

        // The foreground image, used to create a dark effect.
        public RenderTexture Darkness;
        public List<Sprite> Lights;
        public View Camera;

        // The program's entry point.
        public static void Main(string[] args)
        {
            // Set game variables.
            Surrounded.Options = Options.Load(args.Length > 0 ? args[0] : "options");
            Surrounded.Icon = new Image(Path.Combine(Environment.CurrentDirectory, "icon.png"));
            Surrounded.RNG = new Random();
            Surrounded.SteamOnline = SteamAPI.Init();
            Surrounded.Instance = new Surrounded();
        }

        // Class constructor.
        public Surrounded() : base(Surrounded.Options.Fullscreen ? VideoMode.DesktopMode : new VideoMode(Surrounded.Options.Width, Surrounded.Options.Height), "Surrounded", Surrounded.Options.Fullscreen ? Styles.Fullscreen : Styles.Default, new ContextSettings(24, 24, Surrounded.Options.AntialiasingLevel))
        {
            // Set window options.
            this.SetFramerateLimit(Surrounded.Options.FramerateLimit);
            this.SetIcon(Surrounded.Icon.Size.X, Surrounded.Icon.Size.Y, Surrounded.Icon.Pixels);
            this.SetMouseCursorVisible(false);
            this.SetVerticalSyncEnabled(Surrounded.Options.VerticalSync);

            // Check Steam API.
            if (Surrounded.SteamOnline)
            {
                Console.WriteLine("Steam is connected!");
                Console.Write("Username: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(SteamFriends.GetPersonaName());
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Steam is not connected.");
            }

            // Window event handlers.
            this.Closed += this.OnClosed;
            this.GainedFocus += this.OnGainedFocus;
            this.LostFocus += this.OnLostFocus;
            this.Resized += this.OnResized;

            // Joystick event handlers.
            this.JoystickButtonPressed += this.OnJoystickButtonPressed;
            this.JoystickButtonReleased += this.OnJoystickButtonReleased;
            this.JoystickConnected += this.OnJoystickConnected;
            this.JoystickDisconnected += this.OnJoystickDisconnected;
            this.JoystickMoved += this.OnJoystickMoved;

            // Keyboard event handlers.
            this.KeyPressed += this.OnKeyPressed;
            this.KeyReleased += this.OnKeyReleased;
            this.TextEntered += this.OnTextEntered;

            // Mouse event handlers.
            this.MouseButtonPressed += this.OnMouseButtonPressed;
            this.MouseButtonReleased += this.OnMouseButtonReleased;
            this.MouseMoved += this.OnMouseMoved;
            this.MouseWheelMoved += this.OnMouseWheelMoved;

            // Create the player.
            this.Player = new Player(this.World);

            // Create foreground images and other effects.
            this.Darkness = new RenderTexture(this.Size.X, this.Size.Y);
            this.Lights = new List<Sprite>();

            // Run the game!
            while (this.IsOpen)
            {
                // Update player input and window events before anything else.
                this.DispatchEvents();

                // Clear screen and foreground.
                this.Clear(new Color(64, 64, 64));
                this.Darkness.Clear(Color.Black);
                this.Lights.Clear();

                // Update player logic.
                this.Player.Update(this);

                // Add lights foreground.
                for (int Light = 0; Light < Lights.Count; ++Light)
                {
                    this.Lights[Light].Draw(this.Darkness, RenderStates.Default);
                }

                // Draw the following in the exact order.
                this.Draw(this.World.LowerLayers, RenderStates.Default);
                this.Draw(this.Player.Sprite, RenderStates.Default);
                this.Draw(this.World.UpperLayers, RenderStates.Default);

                // Draw foreground image.
                this.Darkness.Display();
                this.Draw(new Sprite(this.Darkness.Texture), new RenderStates(BlendMode.Multiply));
                this.Display();
            }
        }

        // Fired when the window is closed.
        private void OnClosed(object sender, EventArgs e)
        {
            Surrounded.Options.Save();
            this.Close();
        }

        // Fired when the window is gained focus.
        private void OnGainedFocus(object sender, EventArgs e)
        {
            
        }

        // Fired when the window loses focus.
        private void OnLostFocus(object sender, EventArgs e)
        {
            
        }

        // Fired when the window resizes.
        private void OnResized(object sender, SizeEventArgs e)
        {
            this.Darkness = new RenderTexture(e.Width, e.Height);
        }

        // Fired when a joystick button is pressed.
        private void OnJoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            
        }

        // Fired when a joystick button is released.
        private void OnJoystickButtonReleased(object sender, JoystickButtonEventArgs e)
        {
            
        }

        // Fired when a joystick is connected.
        private void OnJoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            
        }

        // Fired when a joystick is disconnected.
        private void OnJoystickDisconnected(object sender, JoystickConnectEventArgs e)
        {
            
        }

        // Fired when a joystick is moved.
        private void OnJoystickMoved(object sender, JoystickMoveEventArgs e)
        {
            
        }

        // Fired when a key is pressed.
        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            this.Player.OnKeyPressed(e.Code, e.Shift);
        }

        // Fired when a key is released.
        private void OnKeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Insert)
            {
                // Close the current window.
                Surrounded.Options.Save();
                this.Close();

                // Update the settings.
                Surrounded.Options.Fullscreen = !Surrounded.Options.Fullscreen;
                Surrounded.Instance = new Surrounded();
                
            }
            else if (e.Code == Keyboard.Key.Escape)
            {
                // Close the current window.
                Surrounded.Options.Save();
                this.Close();
            }
            this.Player.OnKeyReleased(e.Code, e.Shift);
        }

        // Fired when a ASCII key is pressed.
        private void OnTextEntered(object sender, TextEventArgs e)
        {
            
        }

        // Fired when a mouse button is pressed.
        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Sound sound = new Sound(new SoundBuffer(Path.Combine(Environment.CurrentDirectory, "files", "sounds", "click.wav")));
            sound.Position = new Vector3f(e.X, e.Y, 0);
            sound.Play();
        }

        // Fired when a mouse button is released.
        private void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            
        }

        // Fired when the mouse moves.
        private void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            
        }

        // Fired when the mouse wheel moves.
        private void OnMouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            
        }
    }
}
