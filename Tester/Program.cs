using System;
using System.Diagnostics;

using DidasUtils;
using DidasUtils.Numerics;
using DidasUtils.ConsoleUI;
using DidasUtils.ErrorCorrection;
using DidasUtils.Data;

namespace Tester
{
    class Program
    {



        static void Main(string[] args)
        {
            BitList list = new BitList(5);
            Console.WriteLine($"Init count {list.Count}");
            Console.WriteLine($"Init cap {list.Capacity}");

            list.Add(true);
            list.Add(true);
            list.Add(false);
            list.Add(true);
            list.Add(true);
            list.Add(true);
            list.Add(true);
            list.Add(true);
            list.Add(true);
            /*
            for (int i = 0; i < list.Count; i++)
                Console.Write(list[i]);
            Console.WriteLine();

            byte[] ser = BitList.Serialize(list);
            for (int i = 0; i < ser.Length; i++)
                Console.Write($"{ser[i]:X2} ");
            Console.WriteLine();

            list.AddRange(new bool[12] { true, true, true, false, false, false, true, true, true, false, false, false });
            Console.WriteLine($"Count 2 {list.Count}");
            Console.WriteLine($"Cap 2 {list.Capacity}");

            ser = BitList.Serialize(list);
            Console.WriteLine(ser.Length);
            for (int i = 0; i < ser.Length; i++)
                Console.Write($"{ser[i]:X2}");
            Console.WriteLine();

            for (int i = 0; i < list.Count; i++)
                Console.Write(list[i]);
            Console.WriteLine();

            BitList newL = BitList.Deserialize(ser);

            for (int i = 0; i < newL.Count; i++)
                Console.Write(newL[i]);
            Console.WriteLine();*/

            byte by = list.GetByte(1);
            Console.WriteLine(by.ToString("X2"));


            Console.ReadLine();

            return;

            Random rdm = new Random();

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine($"Iter {i}");

                int len = rdm.Next(16, 262144);
                byte[] srcBytes = new byte[len];
                rdm.NextBytes(srcBytes);

                ErrorProtectedBlock[] blocks = ErrorProtectedBlock.ProtectData(srcBytes, ErrorProtectedBlock.ErrorProtectionType.Fletcher32, 32768);
                ErrorProtectedBlock[] deserializedBlocks = new ErrorProtectedBlock[blocks.Length];

                for (int b = 0; b < blocks.Length; b++) deserializedBlocks[b] = ErrorProtectedBlock.Deserialize(ErrorProtectedBlock.Serialize(blocks[b]));

                int head = 0; bool bug;
                for (int b = 0; b < deserializedBlocks.Length; b++)
                {
                    bug = false;

                    if (!deserializedBlocks[b].Validate())
                    {
                        Console.WriteLine($"Block {b} of round {i} failed validation.");
                        head += deserializedBlocks[b].data.Length;
                        continue;
                    }

                    for (int t = 0; t < deserializedBlocks[b].data.Length; t++)
                    {
                        if (!bug)
                        {
                            if (deserializedBlocks[b].data[t] != srcBytes[head])
                            {
                                bug = true;
                                Console.WriteLine($"First bug for block {b} of round {i} at byte {t}.");
                            }
                        }

                        head++;
                    }
                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }





        private static CUIWindow window;
        private static bool run = true;

        private static void TestCUI()
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
