// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System.Globalization;
    using System.Resources;
    using System.Threading;

    internal sealed class SR
    {
        internal const string CatAction = "CatAction";
        internal const string CatAppearance = "CatAppearance";
        internal const string CatBehavior = "CatBehavior";
        internal const string CatData = "CatData";
        internal const string CatMouse = "CatMouse";
        internal const string ControlOnClickDescr = "ControlOnClickDescr";
        internal const string ControlOnDoubleClickDescr = "ControlOnDoubleClickDescr";
        internal const string ControlOnMouseDownDescr = "ControlOnMouseDownDescr";
        internal const string ControlOnMouseMoveDescr = "ControlOnMouseMoveDescr";
        internal const string ControlOnMouseUpDescr = "ControlOnMouseUpDescr";
        internal const string ControlTagDescr = "ControlTagDescr";
        internal const string DescriptionMessageManager = "DescriptionMessageManager";
        internal const string MessageManagerBalloonTipIconDescr = "MessageManagerBalloonTipIconDescr";
        internal const string MessageManagerBalloonTipTextDescr = "MessageManagerBalloonTipTextDescr";
        internal const string MessageManagerBalloonTipTitleDescr = "MessageManagerBalloonTipTitleDescr";
        internal const string MessageManagerIconDescr = "MessageManagerIconDescr";
        internal const string MessageManagerMenuDescr = "MessageManagerMenuDescr";
        internal const string MessageManagerMouseClickDescr = "MessageManagerMouseClickDescr";
        internal const string MessageManagerMouseDoubleClickDescr = "MessageManagerMouseDoubleClickDescr";
        internal const string MessageManagerOnBalloonTipClickedDescr = "MessageManagerOnBalloonTipClickedDescr";
        internal const string MessageManagerOnBalloonTipClosedDescr = "MessageManagerOnBalloonTipClosedDescr";
        internal const string MessageManagerOnBalloonTipShownDescr = "MessageManagerOnBalloonTipShownDescr";
        internal const string MessageManagerTextDescr = "MessageManagerTextDescr";
        internal const string MessageManagerVisDescr = "MessageManagerVisDescr";

        private static SR loader;
        private readonly ResourceManager resources;

        internal SR()
            => this.resources = new ResourceManager("MessageManager", this.GetType().Assembly);

        public static ResourceManager Resources => GetLoader().resources;

        private static CultureInfo Culture => null;

        public static string GetString(string name, params object[] args)
        {
            var sR = GetLoader();
            if (sR == null)
            {
                return null;
            }

            var str = sR.resources.GetString(name, Culture);
            if (args != null && args.Length != 0)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i] is string text && text.Length > 1024)
                    {
                        args[i] = text.Substring(0, 1021) + "...";
                    }
                }

                return string.Format(CultureInfo.CurrentCulture, str, args);
            }

            return str;
        }

        public static string GetString(string name)
            => GetLoader()?.resources.GetString(name, Culture);

        public static string GetString(string name, out bool usedFallback)
        {
            usedFallback = false;
            return GetString(name);
        }

        public static object GetObject(string name)
            => GetLoader()?.resources.GetObject(name, Culture);

        private static SR GetLoader()
        {
            if (loader == null)
            {
                var value = new SR();
                _ = Interlocked.CompareExchange(ref loader, value, null);
            }

            return loader;
        }
    }
}
