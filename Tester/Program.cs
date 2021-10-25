using System;

using DidasUtils;
using DidasUtils.Numerics;
using DidasUtils.ConsoleUI;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            CUIWindow window = BuildWindow();

            CUIDrawer.DrawWindows();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                    break;

                if (key.Key == ConsoleKey.LeftArrow)
                    window.SelectPrevTab();
                else if (key.Key == ConsoleKey.RightArrow)
                    window.SelectNextTab();

                CUIDrawer.DrawWindows();
            }
        }

        private static CUIWindow BuildWindow()
        {
            CUIDrawer.Init(new Vector2i(100, 30));

            CUIWindow window = new CUIWindow(new Vector2i(100, 30), Vector2i.Zero);

            CUIPage page = new CUIPage("Main");
            page.AddElement(new CUIText(new Vector2i(0, 0), new Vector2i(20, 1), "This is the main menu."));
            page.AddElement(new CUIText(new Vector2i(8, 8), new Vector2i(20, 1), "Things can have random positions."));
            page.AddElement(new CUIText(new Vector2i(0, 1), new Vector2i(20, 1), "Buttons and input fields coming."));
            page.AddElement(new CUIEmpty(new Vector2i(99, 28), new Vector2i(1,1), ConsoleColor.Red));

            window.Pages.Add(page);
            window.Pages.Add(new CUIPage("Page 2"));
            window.Pages.Add(new CUIPage("Page 3"));

            CUIDrawer.AddWindow(window);

            return window;
        }
    }
}
