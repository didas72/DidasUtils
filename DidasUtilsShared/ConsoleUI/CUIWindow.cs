using System;
using System.Collections.Generic;

using DidasUtils;
using DidasUtils.Extensions;
using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public class CUIWindow
    {
        public List<CUIPage> Pages { get; }
        public Vector2i WindowPos { get; private set; }
        public Vector2i WindowSize { get; private set; }
        public ConsoleColor Background { get; set; }
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



        public void SelectPrevTab() => selectedPage--;
        public void SelectNextTab() => selectedPage++;



        public void Draw()
        {
            if (Pages.Count <= 0)
                throw new NoPagesException();

            if (selectedPage < 0)
                selectedPage = Pages.Count - 1;
            else if (selectedPage >= Pages.Count)
                selectedPage = 0;

            DrawPageTabs();
            DrawPage();
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
        private void DrawPage()
        {
            for (int i = 0; i < Pages[selectedPage].Elements.Count; i++)
            {
                try
                {
                    CUIElement element = Pages[selectedPage].Elements[i];
                    Vector2i off = new Vector2i(0, 1);

                    switch (element.Type)
                    {
                        case CUIElement.ElementType.Empty:
                            CUIEmpty empty = (CUIEmpty)element;

                            string clear = " \n".Loop(empty.Size.y);
                            CUIDrawer.DrawLocal(this, clear, ConsoleColor.White, empty.Color, empty.Position + off, empty.Size);
                            break;

                        case CUIElement.ElementType.Text:
                            CUIText text = (CUIText)element;

                            CUIDrawer.DrawLocal(this, text.Content, text.ForegroundColor, text.BackgroundColor, text.Position + off);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }



    public class NoPagesException : Exception
    {

    }
}
