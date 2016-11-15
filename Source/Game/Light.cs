using System;
using System.IO;

using SFML.Graphics;
using SFML.System;

namespace Surrounded.Source.Game
{
    public class Light
    {
        // The drawn image itself.
        public Sprite Sprite;
        
        // Class constructor.
        public Light(Vector2f position, Color color, float size = 1.0F, string fileName = "circle")
        {
            this.Sprite = new Sprite(new Texture(Path.Combine(Environment.CurrentDirectory, "files", "textures", "light", fileName + ".png")));
            this.Sprite.Origin = new Vector2f(64, 64);
            this.Sprite.Position = position;
            this.Sprite.Color = color;
            this.Sprite.Scale = new Vector2f(size, size);
        }

        // Secondary class constructor.
        public Light(Sprite sprite)
        {
            this.Sprite = sprite;
        }
    }
}
