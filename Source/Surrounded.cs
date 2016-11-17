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
        // These are required by game instance, but should not be a part of it.
        public static Options Options;
        public static Image Icon;
        public static Random RNG;
        public static bool SteamOnline;

        // This is the game instance itself, and therefore should be accessible to all.
        public static Surrounded Instance;
        
        // These are static in order to be safe while the game window is closed.
        public static Player Player;
        public static Map Map;

        // This will be removed in the future.
        public bool DrawDarkness = true;

        // The foreground image, used to create a dark effect.
        public RenderTexture Darkness;
        public List<Light> Lights;

        // Frame timers.
        public Clock FrameTimer;
        public uint Frames;

        // The program's entry point.
        public static void Main(string[] args)
        {
            // Set the static game variables.
            Surrounded.Options = Options.Load(args.Length > 0 ? args[0] : "options");
            Surrounded.Icon = new Image(Path.Combine(Environment.CurrentDirectory, "icon.png"));
            Surrounded.RNG = new Random();

            // Check the Steam API.
            Surrounded.SteamOnline = SteamAPI.Init();
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
            
            // Load up the other game stuff.
            Surrounded.Player = new Player();
            Surrounded.Map = new Map(Surrounded.Player, "default");

            // Start the game.
            Surrounded.Instance = new Surrounded();
        }

        // Class constructor.
        public Surrounded() : base(Surrounded.Options.Fullscreen ? VideoMode.DesktopMode : new VideoMode(Surrounded.Options.Width, Surrounded.Options.Height), "Surrounded", Surrounded.Options.Fullscreen ? Styles.Fullscreen : Styles.Close, new ContextSettings(24, 24, Surrounded.Options.AntialiasingLevel))
        {
            // Set window options.
            this.SetFramerateLimit(Surrounded.Options.FramerateLimit);
            this.SetIcon(Surrounded.Icon.Size.X, Surrounded.Icon.Size.Y, Surrounded.Icon.Pixels);
            this.SetKeyRepeatEnabled(true);
            this.SetMouseCursorVisible(true);
            this.SetTitle("Surrounded [FPS: Loading...]");
            this.SetVerticalSyncEnabled(Surrounded.Options.VerticalSync);

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

            // Create foreground images and other effects.
            this.Darkness = new RenderTexture(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
            this.Lights = new List<Light>();

            // Create frame timers and counters.
            this.FrameTimer = new Clock();
            this.Frames = 0;

            // Run the game!
            while (this.IsOpen)
            {
                // Update player input and window events before anything else.
                this.DispatchEvents();

                // Clear lights before logic because those work hand in hand.
                for (int i = 0; i < Lights.Count; ++i)
                {
                    this.Lights[i].Dispose();
                }
                this.Lights.Clear();

                // Update map logic.
                Surrounded.Map.Update(this);

                // Begin rendering by first and foremost, clearing the screens.
                this.Clear(Color.Black);
                this.Darkness.Clear(Color.Black);

                // Draw the map.
                Surrounded.Map.Draw(this, this.Darkness);

                // Draw the lights.
                for (int i = 0; i < Lights.Count; ++i)
                {
                    this.Darkness.Draw(Lights[i], RenderStates.Default);
                }

                // Draw foreground image.
                this.Darkness.Display();
                if (this.DrawDarkness)
                {
                    this.Draw(new Sprite(this.Darkness.Texture), new RenderStates(BlendMode.Multiply));
                }

                // Draw the map name.
                Surrounded.Map.Name.Draw(this, RenderStates.Default);

                // Draw the screen.
                this.Display();

                // Get FPS.
                if (this.FrameTimer.ElapsedTime.AsMilliseconds() > 1000)
                {
                    this.FrameTimer.Restart();
                    this.SetTitle("Surrounded [FPS: " + this.Frames + "]");
                    this.Frames = 0;
                }
                else
                {
                    ++this.Frames;
                }
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
            else if (e.Code == Keyboard.Key.BackSpace)
            {
                this.DrawDarkness = !this.DrawDarkness;
            }
        }

        // Fired when a ASCII key is pressed.
        private void OnTextEntered(object sender, TextEventArgs e)
        {
            
        }

        // Fired when a mouse button is pressed.
        private void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {

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
