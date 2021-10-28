using System;

using DidasUtils;
using DidasUtils.Numerics;
using DidasUtils.ConsoleUI;

namespace Tester
{
    class Program
    {
        private static CUIWindow window;
        private static bool run = true;


        static void Main(string[] args)
        {
            window = BuildWindow();

            CUIDrawer.DrawWindows();

            while (run)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                    run = false;

                if (key.Key == ConsoleKey.LeftArrow)
                    window.SelectPrevTab();
                else if (key.Key == ConsoleKey.RightArrow)
                    window.SelectNextTab();
                else if (key.Key == ConsoleKey.DownArrow)
                    window.SelectNextInteractable();
                else if (key.Key == ConsoleKey.UpArrow)
                    window.SelectPrevInteractable();
                else
                    window.Interact(key);

                CUIDrawer.DrawWindows();
            }
        }

        private static CUIWindow BuildWindow()
        {
            CUIDrawer.Init(new Vector2i(100, 30));

            CUIWindow window = new CUIWindow(new Vector2i(100, 30), Vector2i.Zero);

            CUIPage page = new CUIPage("Main");
            page.AddElement(new CUIText(new Vector2i(0, 0), "This is the main menu."));
            page.AddElement(new CUIText(new Vector2i(8, 1), "Things can have random positions."));
            page.AddElement(new CUIText(new Vector2i(0, 2), "Buttons and input fields coming."));

            page.AddElement(new CUISelectable(new Vector2i(0, 4), "This is a selectable."));
            page.AddElement(new CUISelectable(new Vector2i(0, 5), "This is another selectable."));
            page.AddElement(new CUIText(new Vector2i(0, 6), "/\\ These do nothing."));

            CUISelectable sel = new CUISelectable(new Vector2i(0, 8), "I change to next tab.");
            sel.OnInteract += OnInteractedNext;
            page.AddElement(sel);
            sel = new CUISelectable(new Vector2i(0, 9), "I change to prev tab.");
            sel.OnInteract += OnInteractedPrev;
            page.AddElement(sel);
            sel = new CUISelectable(new Vector2i(0, 10), "I exit the app (ESC).");
            sel.OnInteract += OnInteractedExit;
            page.AddElement(sel);

            CUIInput inp = new CUIInput(new Vector2i(0, 12), new Vector2i(20, 1), "");
            inp.OnInteract += OnInteractedInput;
            page.AddElement(inp);

            window.AddPage(page);
            window.AddPage(new CUIPage("Page 2"));
            window.AddPage(new CUIPage("Page 3"));

            CUIDrawer.AddWindow(window);

            return window;
        }



        private static void OnInteractedNext(object sender, InteractEventArgs e)
        {
            window.SelectNextTab();
        }
        private static void OnInteractedPrev(object sender, InteractEventArgs e)
        {
            window.SelectPrevTab();
        }
        private static void OnInteractedExit(object sender, InteractEventArgs e)
        {
            run = false;
        }

        private static CUIText inpText = null;

        private static void OnInteractedInput(object sender, InteractEventArgs e)
        {
            if (e.arguments[1] == "\n" || e.arguments[1] == "\r")
            {
                if (inpText == null)
                {
                    inpText = new CUIText(new Vector2i(0, 0), e.arguments[0]);
                    CUIPage page = new CUIPage("Outp");
                    page.AddElement(inpText);
                    window.AddPage(page);
                }
                else
                {
                    inpText.SetContent(e.arguments[0]);
                }
            }
        }
    }
}
