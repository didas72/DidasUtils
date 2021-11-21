using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DidasUtils;
using DidasUtils.Extensions;
using DidasUtils.Numerics;

namespace DidasUtils.ConsoleUI
{
    public static class CUIDrawer
    {
        private static List<CUIWindow> Windows;
        public static ReadOnlyCollection<CUIWindow> RWindows { get => Windows.AsReadOnly(); }
        private static Vector2i consoleSize;
        public static Vector2i ConsoleSize { get => consoleSize; set
            {
                consoleSize = value;
                Console.SetWindowSize(consoleSize.x, consoleSize.y);
                Console.SetBufferSize(consoleSize.x + 1, consoleSize.y + 1);
            } }



        public static void Init(Vector2i consoleSize)
        {
            Windows = new List<CUIWindow>();
            ConsoleSize = consoleSize;
            Console.CursorVisible = false;
        }
        public static void AddWindow(CUIWindow window)
        {
            Windows.Add(window);
        }
        public static void DrawWindows()
        {
            Clear();

            foreach (CUIWindow window in Windows)
            {
                window.Draw();
            }
        }
        private static void Clear()
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Clear();
        }



        public static void DrawLocal(CUIWindow sender, string content, ConsoleColor foregroundColor, ConsoleColor backgroundColor, Vector2i position) => DrawLocal(sender, content, foregroundColor, backgroundColor, position, new Vector2i(-1, -1));
        public static void DrawLocal(CUIWindow sender, string content, ConsoleColor foregroundColor, ConsoleColor backgroundColor, Vector2i position, Vector2i size)
        {
            Vector2i localPos = position + sender.WindowPos;
            int maxSX = size.x == -1 ? Math.Min(sender.WindowSize.x - localPos.x, content.Length) : Math.Min(sender.WindowSize.x - localPos.x, size.x);
            int maxSY = size.y == -1 ? Math.Min(sender.WindowSize.y - localPos.y, content.Length) : Math.Min(sender.WindowSize.y - localPos.y, size.y);
            Vector2i maxSpace = new Vector2i(maxSX, maxSY);

            if (maxSpace.x <= 0)
                return;
            if (maxSpace.y <= 0)
                return;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            string[] lines = content.Trim('\n').Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (i >= maxSpace.y)
                    break;

                Console.SetCursorPosition(localPos.x, localPos.y + i);
                Console.Write(lines[i].SetLength(maxSpace.x));
            }
        }
    }
}
