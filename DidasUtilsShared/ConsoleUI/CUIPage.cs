using System;
using System.Collections.Generic;

namespace DidasUtils.ConsoleUI
{
    public class CUIPage
    {
        public string Title { get; private set; }
        public List<CUIElement> Elements { get; }



        public CUIPage(string title)
        {
            Title = title;
            Elements = new List<CUIElement>();
        }



        public void AddElement(CUIElement element) => Elements.Add(element);



        /*public void Draw()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                try
                {
                    switch (Elements[i].Type)
                    {
                        case CUIElement.ElementType.Empty:
                            break;

                        case CUIElement.ElementType.Text:
                            CUIText text = (CUIText)Elements[i];

                            Console.BackgroundColor = text.BackgroundColor;
                            Console.ForegroundColor = text.ForegroundColor;

                            Console.CursorTop = text.Position.y + 1;
                            Console.CursorLeft = text.Position.x;

                            int len = Mathf.Clamp(Console.WindowWidth - Console.CursorLeft, 0, text.Content.Length);
                            Console.Write(text.Content.Substring(0, len));
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
        }*/
    }
}
