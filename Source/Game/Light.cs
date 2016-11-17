using System;
using System.IO;

using SFML.Graphics;
using SFML.System;

namespace Surrounded.Source.Game
{
    public class Light : Sprite
    {
        // Class constructor.
        public Light(Vector2f position, Color color, float size = 1.0F, string shape = "circle") : base(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "light", shape + ".png")))
        {
            this.Origin = new Vector2f(64, 64);
            this.Position = position;
            this.Color = color;
            this.Scale = new Vector2f(size, size);
        }

        // Converts JSON object to normal object.
        public static Light ConvertFromJson(JSON.Light json)
        {
            return new Light(json.Position, json.Color, json.Size, json.Shape);
        }
    }
}
