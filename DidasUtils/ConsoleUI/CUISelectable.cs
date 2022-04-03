using System;

using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public class CUISelectable : CUIElement, ICUIInteractable
    {
        public string Content { get; private set; }
        public ConsoleColor ForegroundColor { get; private set; }
        public ConsoleColor BackgroundColor { get; private set; }
        public ConsoleColor SelForegroundColor { get; private set; }
        public ConsoleColor SelBackgroundColor { get; private set; }



        public EventHandler OnSelect { get; set; }
        public EventHandler<InteractEventArgs> OnInteract { get; set; }



        public CUISelectable(Vector2i position, string content)
        {
            //a weird bug makes this return all 0s or null if I inline it and call a diff constructor, so here this is... :/
            Type = ElementType.Selectable;
            Position = position;
            Size = new Vector2i(-1, -1);
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            SelForegroundColor = ConsoleColor.Black;
            SelBackgroundColor = ConsoleColor.White;
            Content = content;
        }
        public CUISelectable(Vector2i position, Vector2i size) => new CUIText(position, size, string.Empty);
        public CUISelectable(Vector2i position, Vector2i size, string content)
        {
            Type = ElementType.Selectable;
            Position = position;
            Size = size;
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Black;
            SelForegroundColor = ConsoleColor.Black;
            SelBackgroundColor = ConsoleColor.White;
            Content = content;
        }



        public void ClearContent() => Content = string.Empty;
        public void SetContent(string content) => Content = content;
        public void SetColors(ConsoleColor foreground, ConsoleColor background)
        {
            ForegroundColor = foreground;
            BackgroundColor = background;
        }
        public void SetSelColors(ConsoleColor foreground, ConsoleColor background)
        {
            SelForegroundColor = foreground;
            SelBackgroundColor = background;
        }
        public void SetForegroundColor(ConsoleColor color) => ForegroundColor = color;
        public void SetBackgroundColor(ConsoleColor color) => BackgroundColor = color;
        public void SetSelForegroundColor(ConsoleColor color) => SelForegroundColor = color;
        public void SetSelBackgroundColor(ConsoleColor color) => SelBackgroundColor = color;



        public void Interact(ConsoleKeyInfo info)
        {
            InteractEventArgs e = new(Type);

            OnInteract?.Invoke(this, e);
        }
        public void Select()
        {
            OnSelect?.Invoke(this, EventArgs.Empty);
        }
    }
}
