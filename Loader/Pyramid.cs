using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabRat
{
    /// <summary>
    /// A class for rendering a pyramid
    /// </summary>
    public class Pyramid
    {
        /// <summary>
        /// The vertices of the pyramid
        /// </summary>
        VertexBuffer vertices;

        /// <summary>
        /// The vertex indices of the pyramid
        /// </summary>
        IndexBuffer indices;

        /// <summary>
        /// The effect to use rendering the pyramid
        /// </summary>
        BasicEffect effect;

        /// <summary>
        /// The game this pyramid belongs to 
        /// </summary>
        Game game;

        Color _baseColor = new Color(19, 19, 19);

        /// <summary>
        /// Constructs a pyramid instance
        /// </summary>
        /// <param name="game">The game that is creating the pyramid</param>
        public Pyramid(LabRatGame game)
        {
            this.game = game;
            InitializeVertices();
            InitializeIndices();
            InitializeEffect();
        }

        /// <summary>
        /// Updates the Pyramid
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float angle = (float)gameTime.TotalGameTime.TotalSeconds * .1f;

            effect.View = Matrix.CreateRotationY(angle) * Matrix.CreateLookAt(
                new Vector3(0, 5f, -15f),
                Vector3.Zero,
                Vector3.Up
            );
        }

        /// <summary>
        /// Initialize the vertex buffer for the pyramid
        /// </summary>
        public void InitializeVertices()
        {
            float size = .75f; 
            float height = 1f;
            var vertexData = new VertexPositionColor[]
            {
                new VertexPositionColor() { Position = new Vector3(-size, 0, -size), Color = _baseColor },
                new VertexPositionColor() { Position = new Vector3(size, 0, -size), Color = _baseColor },
                new VertexPositionColor() { Position = new Vector3(size, 0, size), Color = _baseColor },
                new VertexPositionColor() { Position = new Vector3(-size, 0, size), Color = _baseColor },

                new VertexPositionColor() { Position = new Vector3(0, height, 0), Color = Color.DarkGray }
            };
            vertices = new VertexBuffer(
                game.GraphicsDevice,
                typeof(VertexPositionColor),
                vertexData.Length,
                BufferUsage.None
            );
            vertices.SetData(vertexData);
        }

        /// <summary>
        /// Initializes the index buffer for the pyramid
        /// </summary>
        public void InitializeIndices()
        {
            var indexData = new short[]
            {
                // Base
                0, 1, 2,
                0, 2, 3, 
                // Sides
                0, 1, 4,
                1, 2, 4,
                2, 3, 4,
                3, 0, 4
            };
            indices = new IndexBuffer(
                game.GraphicsDevice,
                IndexElementSize.SixteenBits,
                indexData.Length,
                BufferUsage.None
            );
            indices.SetData(indexData);
        }

        /// <summary>
        /// Initializes the BasicEffect to render our pyramid
        /// </summary>
        void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 15),
                Vector3.Zero,
                Vector3.Up
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                game.GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                100.0f
            );
            effect.VertexColorEnabled = true;
        }

        /// <summary>
        /// Draws the Pyramid
        /// </summary>
        public void Draw()
        {
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.Indices = indices;
            game.GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                indices.IndexCount / 3
            );
        }
    }
}
