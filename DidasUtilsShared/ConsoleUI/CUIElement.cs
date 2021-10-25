using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public abstract class CUIElement
    {
        public Vector2i Position { get; protected set; }
        public Vector2i Size { get; protected set; } = new Vector2i(-1, -1);
        public ElementType Type { get; protected set; }



        public void SetPosition(Vector2i position) => Position = position;



        public enum ElementType
        {
            Empty,
            Text,
            Selectable,
            Input,
        }
    }
}
