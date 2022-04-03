using System;

using DidasUtils;
using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public class CUIInput : CUIElement, ICUIInteractable
    {
        public string Content { get; private set; }
        public ConsoleColor ForegroundColor { get; private set; }
        public ConsoleColor BackgroundColor { get; private set; }
        public ConsoleColor SelForegroundColor { get; private set; }
        public ConsoleColor SelBackgroundColor { get; private set; }



        public EventHandler OnSelect { get; set; }
        public EventHandler<InteractEventArgs> OnInteract { get; set; }



        public CUIInput(Vector2i position, string content)
        {
            //a weird bug makes this return all 0s or null if I inline it and call a diff constructor, so here this is... :/
            Type = ElementType.Input;
            Position = position;
            Size = new Vector2i(-1, -1);
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            SelForegroundColor = ConsoleColor.Black;
            SelBackgroundColor = ConsoleColor.White;
            Content = content;
        }
        public CUIInput(Vector2i position, Vector2i size) => new CUIText(position, size, string.Empty);
        public CUIInput(Vector2i position, Vector2i size, string content)
        {
            Type = ElementType.Input;
            Position = position;
            Size = size;
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            SelForegroundColor = ConsoleColor.Black;
            SelBackgroundColor = ConsoleColor.White;
            Content = content;
        }



        public void Interact(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Backspace && Content.Length > 0)
                Content = Content[0..^1];
            else if (keyInfo.KeyChar == ' ')
                Content += ' ';
            else if (!string.IsNullOrWhiteSpace(keyInfo.KeyChar.ToString()))
                Content += keyInfo.KeyChar;

            InteractEventArgs e = new(Type, new string[] { Content, keyInfo.KeyChar.ToString() });

            OnInteract?.Invoke(this, e);
        }

        public void Select()
        {
            OnSelect?.Invoke(this, EventArgs.Empty);
        }
    }
}
