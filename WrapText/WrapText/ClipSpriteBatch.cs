using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrapText
{
    public class ClipSpriteBatch
    {
        private readonly Rectangle _area;
        private readonly RasterizerState _rasterizerState;
        private readonly SpriteBatch _spriteBatch;
        private Rectangle _preservedArea;

        public ClipSpriteBatch(Rectangle area, SpriteBatch spriteBatch)
        {
            _area = area;
            _spriteBatch = spriteBatch;
            _rasterizerState = new RasterizerState { ScissorTestEnable = true };
        }

        public void Begin()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, _rasterizerState);
            
            _preservedArea = _spriteBatch.GraphicsDevice.ScissorRectangle;
            _spriteBatch.GraphicsDevice.ScissorRectangle = _area;
        }

        public void End()
        {
            _spriteBatch.GraphicsDevice.ScissorRectangle = _preservedArea;
            _spriteBatch.End();
        }
    }
}
