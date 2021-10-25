using System;
using System.Collections.Generic;

namespace DidasUtils.ConsoleUI
{
    public class CUIPage
    {
        public string Title { get; private set; }
        private List<CUIElement> Elements;
        private List<ICUIInteractable> Interactables;



        private int selectedInteractable;



        public CUIPage(string title)
        {
            Title = title;

            Elements = new List<CUIElement>();
            Interactables = new List<ICUIInteractable>();
        }



        public void AddElement(CUIElement element)
        {
            Elements.Add(element);

            if (element is ICUIInteractable interactable) Interactables.Add(interactable);
        }

        

        public void SelectNextInteractable()
        {
            selectedInteractable++;

            if (!TryGetSelectedInteractable(out ICUIInteractable interactable))
                return;

            interactable.Select();
        }
        public void SelectPrevInteractable()
        {
            selectedInteractable--;

            if (!TryGetSelectedInteractable(out ICUIInteractable interactable))
                return;

            interactable.Select();
        }
        public void Interact(ConsoleKeyInfo keyInfo)
        {
            if (!TryGetSelectedInteractable(out ICUIInteractable interactable)) return;

            interactable.Interact(keyInfo);
        }



        public CUIElement[] GetElements() => Elements.ToArray();
        public bool TryGetSelectedInteractable(out ICUIInteractable interactable)
        {
            interactable = null;

            if (Interactables.Count <= 0)
                return false;

            if (selectedInteractable < 0)
                selectedInteractable = Interactables.Count - 1;
            else if (selectedInteractable >= Interactables.Count)
                selectedInteractable = 0;

            interactable = Interactables[selectedInteractable];

            return true;
        }
    }
}
