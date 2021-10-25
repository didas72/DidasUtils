using System;

using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public class CUIText : CUIElement
    {
        public string Content { get; private set; }
        public ConsoleColor ForegroundColor { get; private set; }
        public ConsoleColor BackgroundColor { get; private set; }



        public CUIText(Vector2i position, string content)
        {
            //a weird bug makes this return all 0s or null if I inline it and call a diff constructor, so here this is... :/
            Type = ElementType.Text;
            Position = position;
            Size = new Vector2i(-1, -1);
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            Content = content;
        }
        public CUIText(Vector2i position, Vector2i size) => new CUIText(position, size, string.Empty);
        public CUIText(Vector2i position, Vector2i size, string content)
        {
            Type = ElementType.Text;
            Position = position;
            Size = size;
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            Content = content;
        }



        public void ClearContent() => Content = string.Empty;
        public void SetContent(string content) => Content = content;
        public void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            ForegroundColor = foreground;
            BackgroundColor = background;
        }
        public void SetForegroundColor(ConsoleColor color) => ForegroundColor = color;
        public void SetBackgroundColor(ConsoleColor color) => BackgroundColor = color;
    }
}
