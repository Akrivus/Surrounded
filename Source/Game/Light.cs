using System;
using System.IO;

using SFML.Graphics;
using SFML.System;

namespace Surrounded.Source.Game
{
    public class Light : Sprite
    {
        // Class constructor.
        public Light(Vector2f position, Color color, float size = 1.0F, string fileName = "circle") : base(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "light", fileName + ".png")))
        {
            this.Origin = new Vector2f(64, 64);
            this.Position = position;
            this.Color = color;
            this.Scale = new Vector2f(size, size);
        }
    }
}
