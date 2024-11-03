using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabRat
{
    public class Tilemap
    {
        public Vector2 Offset = Vector2.Zero;
        public List<BoundingRectangle> Colliders = new();
        public int Size => _tileHeight;
        public int MapPixelWidth => _mapWidth * (_tileWidth - DoublePadding);

        public int MapPixelHeight => _mapHeight * (_tileHeight - DoublePadding);

        public Vector2 StartPosition { get; private set; }

        public Vector2 EndPosition { get; private set; }

        /// <summary>
        /// Dimensions
        /// </summary>
        int _tileWidth, _tileHeight, _mapWidth, _mapHeight;

        /// <summary>
        /// tileset texture
        /// </summary>
        Texture2D _tilesetTexture;

        /// <summary>
        /// tile info in the set
        /// </summary>
        Rectangle[] _tiles;

        /// <summary>
        /// tilemap data
        /// </summary>
        int[] _map;

        string _filename;

        const int Padding = 1;
        const int DoublePadding = 2;

        bool _collisionsSet = false;

        public Tilemap(string filename)
        {
            _filename = filename;
        }

        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));
            var lines = data.Split('\n');

            var tilesetFilename = lines[0].Trim();
            _tilesetTexture = content.Load<Texture2D>(tilesetFilename);
             
            var secondLine = lines[1].Split(',');
            _tileWidth = int.Parse(secondLine[0]);
            _tileHeight = int.Parse(secondLine[1]);

            int tilesetCols = _tilesetTexture.Width / _tileWidth;
            int tilesetRows = _tilesetTexture.Height / _tileHeight;
            _tiles = new Rectangle[tilesetCols * tilesetRows];

            for(int y = 0; y < tilesetRows; y++)
            {
                for(int x = 0; x < tilesetCols; x++)
                {
                    int index = y * tilesetCols + x;
                    _tiles[index] = new Rectangle(x * _tileWidth + Padding, y * _tileHeight + Padding, _tileWidth - DoublePadding, _tileHeight - DoublePadding);
                }
            }
        }

        public void LoadMap(string filename, string root)
        {
            Colliders.Clear();
            _collisionsSet = false;
            string data = File.ReadAllText(Path.Join(root, filename));
            var lines = data.Split('\n');
            var firstLine = lines[0].Split(',');
            _mapWidth = int.Parse(firstLine[0]);
            _mapHeight = int.Parse(firstLine[1]);
            StartPosition = new Vector2((_tileWidth - DoublePadding) * int.Parse(firstLine[2]), (_tileHeight - DoublePadding) * int.Parse(firstLine[3]));
            EndPosition = new Vector2((_tileWidth - DoublePadding) * int.Parse(firstLine[4]), (_tileHeight - DoublePadding) * int.Parse(firstLine[5]));

            string restOfFile = "";
            for(int i = 1; i < lines.Length; i++)
            {
                restOfFile += lines[i];
            }

            var secondLine = restOfFile.Split(',');
            var size = _mapWidth * _mapHeight;
            _map = new int[size];
            for (int i = 0; i < size; i++)
            {
                _map[i] = int.Parse(secondLine[i]);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                for (int x = 0; x < _mapWidth; x++)
                {
                    int index = _map[y * _mapWidth + x] - 1;
                    if (index == -1) continue;
                    var pos = new Vector2(x * (_tileWidth - DoublePadding), y * (_tileHeight - DoublePadding)) + Offset;
                    spriteBatch.Draw(_tilesetTexture, pos, _tiles[index], Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    if (!_collisionsSet)
                    {
                        Colliders.Add(new BoundingRectangle(pos, _tileWidth - DoublePadding, _tileHeight - DoublePadding));
                    }
                }
            }
            _collisionsSet = true;
        }
    }
}
