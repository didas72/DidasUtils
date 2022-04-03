using System;

namespace DidasUtils.ConsoleUI
{
    public interface ICUIInteractable
    {
        EventHandler OnSelect { get; set; }
        EventHandler<InteractEventArgs> OnInteract { get; set; }



        void Interact(ConsoleKeyInfo keyInfo);
        void Select();
    }



    public class InteractEventArgs : EventArgs
    {
        public CUIElement.ElementType type;
        public string[] arguments;

        public InteractEventArgs(CUIElement.ElementType type)
        {
            this.type = type;
            arguments = Array.Empty<string>();
        }
        public InteractEventArgs(CUIElement.ElementType type, string[] args)
        {
            this.type = type;
            arguments = args;
        }
    }
}
