using System;
using System.Collections.Generic;

using DidasUtils;
using DidasUtils.Extensions;
using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public class CUIWindow
    {
        private List<CUIPage> Pages;
        public Vector2i WindowPos { get; private set; }
        public Vector2i WindowSize { get; private set; }
        public ConsoleColor TabForegroundColor { get; set; }
        public ConsoleColor TabBackgroundColor { get; set; }
        public ConsoleColor SelTabForegroundColor { get; set; }
        public ConsoleColor SelTabBackgroundColor { get; set; }
        public ConsoleColor TabSepForegroundColor { get; set; }
        public ConsoleColor TabSepBackgroundColor { get; set; }



        private int selectedPage;



        public CUIWindow(Vector2i size) => new CUIWindow(size, Vector2i.Zero);
        public CUIWindow(Vector2i size, Vector2i position)
        {
            Pages = new List<CUIPage>();

            WindowSize = size;
            WindowPos = position;
            selectedPage = 0;

            TabBackgroundColor = ConsoleColor.DarkGray;
            TabForegroundColor = ConsoleColor.Gray;
            SelTabBackgroundColor = ConsoleColor.DarkGreen;
            SelTabForegroundColor = ConsoleColor.White;
            TabSepBackgroundColor = ConsoleColor.Black;
            TabSepForegroundColor = ConsoleColor.Black;
        }



        public void SelectPrevTab()
        {
            selectedPage--;
            ReclampSelectedPage();
        }
        public void SelectNextTab()
        {
            selectedPage++;
            ReclampSelectedPage();
        }

        public void SelectNextInteractable()
        {
            if (!TryGetValidPage(selectedPage, out CUIPage page))
                return;

            page.SelectNextInteractable();
        }
        public void SelectPrevInteractable()
        {
            if (!TryGetValidPage(selectedPage, out CUIPage page))
                return;

            page.SelectPrevInteractable();
        }
        public void Interact(ConsoleKeyInfo keyInfo)
        {
            if (!TryGetValidPage(selectedPage, out CUIPage page))
                return;

            page.Interact(keyInfo);
        }



        public void AddPage(CUIPage page) => Pages.Add(page);



        public void Draw()
        {
            if (!TryGetValidPage(selectedPage, out CUIPage page))
            {
                CUIDrawer.DrawLocal(this, "No pages.", ConsoleColor.White, ConsoleColor.Black, Vector2i.Zero);
                return;
            }

            DrawPageTabs();
            DrawPage(page);
        }
        private void DrawPageTabs()
        {
            int totalCharsNeeded = 0;

            foreach (CUIPage page in Pages)
            {
                totalCharsNeeded += page.Title.Length + 1; //name plus gap
            }

            totalCharsNeeded--;

            if (totalCharsNeeded > WindowSize.x) //tabs don't fit
            {
                throw new NotImplementedException();

                /*int charsPerNormTab = totalCharsNeeded;

                int availableChars = charsPerNormTab - Pages[selectedPage].Title.Length - 1;

                charsPerNormTab = Math.Max(availableChars / Pages.Count, 1);
                int spareSpaces = availableChars - (charsPerNormTab * Pages.Count);
                int xHeader = 0;

                for (int i = 0; i < Pages.Count; i++)
                {
                    ConsoleColor back, fore;

                    if (i == selectedPage)
                    {
                        back = SelTabBackgroundColor;
                        fore = SelTabForegroundColor;
                    }
                    else
                    {
                        back = TabBackgroundColor;
                        fore = TabForegroundColor;
                    }

                    int tLen = Math.Min(Pages[i].Title.Length, charsPerNormTab);

                    if (spareSpaces > 0)
                    {
                        DrawLocal(Pages[i].Title.SetLength(tLen + 1), fore, back, new Vector2i(xHeader, 0));

                        xHeader += tLen + 1;
                        spareSpaces--;
                    }
                    else
                    {
                        DrawLocal(Pages[i].Title.SetLength(tLen), fore, back, new Vector2i(xHeader, 0));

                        xHeader += tLen;
                    }

                    back = TabSepBackgroundColor;
                    fore = TabSepForegroundColor;
                    DrawLocal(" ", fore, back, new Vector2i(xHeader++, 0));
                }*/
            }
            else
            {
                int xHeader = 0;

                for (int i = 0; i < Pages.Count; i++)
                {
                    ConsoleColor back, fore;

                    if (i == selectedPage)
                    {
                        back = SelTabBackgroundColor;
                        fore = SelTabForegroundColor;
                    }
                    else
                    {
                        back = TabBackgroundColor;
                        fore = TabForegroundColor;
                    }

                    CUIDrawer.DrawLocal(this, Pages[i].Title, fore, back, new Vector2i(xHeader, 0), new Vector2i(Pages[i].Title.Length, 1));

                    xHeader += Pages[i].Title.Length;

                    back = TabSepBackgroundColor;
                    fore = TabSepForegroundColor;
                    CUIDrawer.DrawLocal(this, " ", fore, back, new Vector2i(xHeader++, 0), new Vector2i(1, 1));
                }
            }
        }
        private void DrawPage(CUIPage page)
        {
            CUIElement[] elements = page.GetElements();

            for (int i = 0; i < elements.Length; i++)
            {
                try
                {
                    CUIElement element = elements[i];
                    Vector2i offset = new Vector2i(0, 1);

                    DrawElement(element, offset);
                }
                catch { }
            }
        }
        private void DrawElement(CUIElement element, Vector2i offset)
        {
            bool isSelectedInteractable = false;

            if (element is ICUIInteractable interactable)
            {
                Pages[selectedPage].TryGetSelectedInteractable(out ICUIInteractable selectedInteractable);

                if (interactable == selectedInteractable)
                    isSelectedInteractable = true;
            }

            switch (element.Type)
            {
                case CUIElement.ElementType.Empty:
                    CUIEmpty empty = (CUIEmpty)element;

                    string clear = " \n".Loop(empty.Size.y);
                    CUIDrawer.DrawLocal(this, clear, ConsoleColor.White, empty.Color, empty.Position + offset, empty.Size);
                    break;

                case CUIElement.ElementType.Text:
                    CUIText text = (CUIText)element;

                    CUIDrawer.DrawLocal(this, text.Content, text.ForegroundColor, text.BackgroundColor, text.Position + offset, text.Size);
                    break;

                case CUIElement.ElementType.Selectable:
                    CUISelectable selectable = (CUISelectable)element;

                    if (!isSelectedInteractable)
                        CUIDrawer.DrawLocal(this, selectable.Content, selectable.ForegroundColor, selectable.BackgroundColor, selectable.Position + offset, selectable.Size);
                    else
                        CUIDrawer.DrawLocal(this, selectable.Content, selectable.SelForegroundColor, selectable.SelBackgroundColor, selectable.Position + offset, selectable.Size);
                    break;

                case CUIElement.ElementType.Input:
                    CUIInput input = (CUIInput)element;

                    if (!isSelectedInteractable)
                        CUIDrawer.DrawLocal(this, input.Content, input.ForegroundColor, input.BackgroundColor, input.Position + offset, input.Size);
                    else

                        CUIDrawer.DrawLocal(this, input.Content, input.SelForegroundColor, input.SelBackgroundColor, input.Position + offset, input.Size);
                    break;

                default:
                    break;
            }
        }



        private bool TryGetValidPage(int index, out CUIPage page)
        {
            page = null;

            if (Pages.Count <= 0)
                return false;

            if (index < 0)
                index = Pages.Count - 1;
            else if (selectedPage >= Pages.Count)
                index = 0;

            page = Pages[index];

            return true;
        }
        private void ReclampSelectedPage()
        {
            if (selectedPage < 0)
                selectedPage = Pages.Count - 1;
            else if (selectedPage >= Pages.Count)
                selectedPage = 0;
        }
    }



    public class NoPagesException : Exception
    {

    }
}
