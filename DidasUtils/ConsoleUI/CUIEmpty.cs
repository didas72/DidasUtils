using System;
using System.Collections.Generic;
using System.Text;

using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public class CUIEmpty : CUIElement
    {
        public ConsoleColor Color { get; private set; }



        public CUIEmpty(Vector2i position) => new CUIEmpty(position, new Vector2i(-1, -1), ConsoleColor.White);
        public CUIEmpty(Vector2i position, Vector2i size) => new CUIEmpty(position, size, ConsoleColor.White);
        public CUIEmpty(Vector2i position, Vector2i size, ConsoleColor color)
        {
            Type = ElementType.Empty;
            Position = position;
            Size = size;
            Color = color;
        }



        public void SetColor(ConsoleColor color) => Color = color;
    }
}
