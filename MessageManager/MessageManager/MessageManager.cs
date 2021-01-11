// Copyright (c) 2018-2020, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;

    /// <summary>
    /// A generic MessageBox manager.
    /// </summary>
    // Seems that if I use component; I cant customize the renderer to the menustrip.
    [DefaultProperty("Text")]
    [DefaultEvent("MouseDoubleClick")]
    [ToolboxItemFilter("MessageManager")]
    [SRDescription(SR.DescriptionMessageManager)]
    public class MessageManager : /*Component*/Control
    {
        private static NotifyIcon notifyIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class.
        /// </summary>
        [SuppressMessage("Major Code Smell", "S3010:Static fields should not be updated in constructors", Justification = "Needed here.")]
        public MessageManager()
        {
            notifyIcon?.Dispose();
            notifyIcon = new NotifyIcon();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class
        /// with the specified container.
        /// </summary>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container for the internal
        /// <see cref="NotifyIcon"/> control.
        /// </param>
        [SuppressMessage("Major Code Smell", "S3010:Static fields should not be updated in constructors", Justification = "Needed here.")]
        public MessageManager(IContainer container)
        {
            notifyIcon?.Dispose();
            notifyIcon = new NotifyIcon(container);
        }

        /// <summary>
        /// Occurs when the balloon tip is clicked.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.MessageManagerOnBalloonTipClickedDescr)]
        public event EventHandler BalloonTipClicked
        {
            add
            {
                notifyIcon.BalloonTipClicked += value;
            }

            remove
            {
                notifyIcon.BalloonTipClicked -= value;
            }
        }

        /// <summary>
        /// Occurs when the balloon tip is closed by the user.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.MessageManagerOnBalloonTipClosedDescr)]
        public event EventHandler BalloonTipClosed
        {
            add
            {
                notifyIcon.BalloonTipClosed += value;
            }

            remove
            {
                notifyIcon.BalloonTipClosed -= value;
            }
        }

        /// <summary>
        /// Occurs when the balloon tip is displayed on the screen.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.MessageManagerOnBalloonTipShownDescr)]
        public event EventHandler BalloonTipShown
        {
            add
            {
                notifyIcon.BalloonTipShown += value;
            }

            remove
            {
                notifyIcon.BalloonTipShown -= value;
            }
        }

        /// <summary>
        /// Occurs when the user clicks the internal icon in the notification area.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.ControlOnClickDescr)]
        public new event EventHandler Click
        {
            add
            {
                notifyIcon.Click += value;
            }

            remove
            {
                notifyIcon.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the user double-clicks the internal icon in the notification area of the taskbar.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.ControlOnDoubleClickDescr)]
        public new event EventHandler DoubleClick
        {
            add
            {
                notifyIcon.DoubleClick += value;
            }

            remove
            {
                notifyIcon.DoubleClick -= value;
            }
        }

        /// <summary>
        /// Occurs when the user clicks the internal <see cref="NotifyIcon"/> with the mouse.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.MessageManagerMouseClickDescr)]
        public new event MouseEventHandler MouseClick
        {
            add
            {
                notifyIcon.MouseClick += value;
            }

            remove
            {
                notifyIcon.MouseClick -= value;
            }
        }

        /// <summary>
        /// Occurs when the user double-clicks the internal <see cref="NotifyIcon"/> with the
        /// mouse.
        /// </summary>
        [SRCategory(SR.CatAction)]
        [SRDescription(SR.MessageManagerMouseDoubleClickDescr)]
        public new event MouseEventHandler MouseDoubleClick
        {
            add
            {
                notifyIcon.MouseDoubleClick += value;
            }

            remove
            {
                notifyIcon.MouseDoubleClick -= value;
            }
        }

        /// <summary>
        /// Occurs when the user presses the mouse button while the pointer is over the internal icon
        /// in the notification area of the taskbar.
        /// </summary>
        [SRCategory(SR.CatMouse)]
        [SRDescription(SR.ControlOnMouseDownDescr)]
        public new event MouseEventHandler MouseDown
        {
            add
            {
                notifyIcon.MouseDown += value;
            }

            remove
            {
                notifyIcon.MouseDown -= value;
            }
        }

        /// <summary>
        /// Occurs when the user moves the mouse while the pointer is over the internal icon in the
        /// notification area of the taskbar.
        /// </summary>
        [SRCategory(SR.CatMouse)]
        [SRDescription(SR.ControlOnMouseMoveDescr)]
        public new event MouseEventHandler MouseMove
        {
            add
            {
                notifyIcon.MouseMove += value;
            }

            remove
            {
                notifyIcon.MouseMove -= value;
            }
        }

        /// <summary>
        /// Occurs when the user releases the mouse button while the pointer is over the
        /// internal icon in the notification area of the taskbar.
        /// </summary>
        [SRCategory(SR.CatMouse)]
        [SRDescription(SR.ControlOnMouseUpDescr)]
        public new event MouseEventHandler MouseUp
        {
            add
            {
                notifyIcon.MouseUp += value;
            }

            remove
            {
                notifyIcon.MouseUp -= value;
            }
        }

        /// <summary>
        /// Gets or sets the text to display on the balloon tip associated with the internal <see cref="NotifyIcon"/>.
        /// </summary>
        /// <returns>
        /// The text to display on the balloon tip associated with the internal <see cref="NotifyIcon"/>.
        /// </returns>
        [SRCategory(SR.CatAppearance)]
        [Localizable(true)]
        [DefaultValue("")]
        [SRDescription(SR.MessageManagerBalloonTipTextDescr)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, " + AssemblyRef.SYSTEMDESIGN, typeof(UITypeEditor))]
        public string BalloonTipText
        {
            get => notifyIcon.BalloonTipText;
            set => notifyIcon.BalloonTipText = value;
        }

        /// <summary>
        /// Gets or sets the icon to display on the balloon tip associated with the internal <see cref="NotifyIcon"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="ToolTipIcon"/> to display on the balloon tip associated with the internal <see cref="NotifyIcon"/>.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        /// The specified value is not a <see cref="ToolTipIcon"/>.
        /// </exception>
        [SRCategory(SR.CatAppearance)]
        [DefaultValue(ToolTipIcon.None)]
        [SRDescription(SR.MessageManagerBalloonTipIconDescr)]
        public ToolTipIcon BalloonTipIcon
        {
            get => notifyIcon.BalloonTipIcon;
            set => notifyIcon.BalloonTipIcon = value;
        }

        /// <summary>
        /// Gets or sets the title of the balloon tip displayed on the internal <see cref="NotifyIcon"/>.
        /// </summary>
        /// <returns>
        /// The text to display as the title of the balloon tip.
        /// </returns>
        [SRCategory(SR.CatAppearance)]
        [Localizable(true)]
        [DefaultValue("")]
        [SRDescription(SR.MessageManagerBalloonTipTitleDescr)]
        public string BalloonTipTitle
        {
            get => notifyIcon.BalloonTipTitle;
            set => notifyIcon.BalloonTipTitle = value;
        }

#if !NETCOREAPP3_1 && !NET5_0
        /// <summary>
        /// Gets or sets the shortcut menu for the internal icon.
        /// </summary>
        /// <returns>
        /// The <see cref="ContextMenu"/> for the internal icon. The default value is null.
        /// </returns>
        [Browsable(false)]
        [DefaultValue(null)]
        [SRCategory(SR.CatBehavior)]
        [SRDescription(SR.MessageManagerMenuDescr)]
        public new ContextMenu ContextMenu
        {
            get => notifyIcon.ContextMenu;
            set => notifyIcon.ContextMenu = value;
        }
#endif

        /// <summary>
        /// Gets or sets the shortcut menu associated with the internal <see cref="NotifyIcon"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="ContextMenuStrip"/> associated with the internal <see cref="NotifyIcon"/>.
        /// </returns>
        [DefaultValue(null)]
        [SRCategory(SR.CatBehavior)]
        [SRDescription(SR.MessageManagerMenuDescr)]
        public new ContextMenuStrip ContextMenuStrip
        {
            get => notifyIcon.ContextMenuStrip;
            set => notifyIcon.ContextMenuStrip = value;
        }

        /// <summary>
        /// Gets or sets the current icon.
        /// </summary>
        /// <returns>
        /// The <see cref="System.Drawing.Icon"/> displayed by the <see cref="NotifyIcon"/> component.
        /// The default value is null.
        /// </returns>
        [SRCategory(SR.CatAppearance)]
        [Localizable(true)]
        [DefaultValue(null)]
        [SRDescription(SR.MessageManagerIconDescr)]
        public Icon Icon
        {
            get => notifyIcon.Icon;
            set => notifyIcon.Icon = value;
        }

        /// <summary>
        /// Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification
        /// area icon.
        /// </summary>
        /// <returns>
        /// The ToolTip text displayed when the mouse pointer rests on a notification area icon.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// ToolTip text is more than 63 characters long.
        /// </exception>
        [SRCategory(SR.CatAppearance)]
        [Localizable(true)]
        [DefaultValue("")]
        [SRDescription(SR.MessageManagerTextDescr)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, " + AssemblyRef.SYSTEMDESIGN, typeof(UITypeEditor))]
        public new string Text
        {
            get => notifyIcon.Text;
            set => notifyIcon.Text = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the icon is visible in the notification
        /// area of the taskbar.
        /// </summary>
        /// <returns>
        /// true if the icon is visible in the notification area; otherwise, false. The default
        /// value is false.
        /// </returns>
        [SRCategory(SR.CatBehavior)]
        [Localizable(true)]
        [DefaultValue(false)]
        [SRDescription(SR.MessageManagerVisDescr)]
        public new bool Visible
        {
            get => notifyIcon.Visible;
            set => notifyIcon.Visible = value;
        }

        /// <summary>
        /// Gets or sets an object that contains data about the <see cref="NotifyIcon"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/> that contains data about the <see cref="NotifyIcon"/>.
        /// </returns>
        [SRCategory(SR.CatData)]
        [Localizable(false)]
        [Bindable(true)]
        [SRDescription(SR.ControlTagDescr)]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public new object Tag
        {
            get => notifyIcon.Tag;
            set => notifyIcon.Tag = value;
        }

        /// <summary>
        /// Shows an MessageBox that is for an Error.
        /// </summary>
        /// <param name="text">The text on the messagebox.</param>
        /// <param name="caption">The title of the messagebox.</param>
        /// <param name="useNotifications">Indicates if this function should show notifications using the input notification icon.</param>
        /// <returns>A new <see cref="DialogResult"/>.</returns>
        public static DialogResult ShowError(string text, string caption, bool useNotifications)
        {
            if (notifyIcon != null && useNotifications)
            {
                notifyIcon.ShowBalloonTip(0, caption, text, ToolTipIcon.Error);
                return DialogResult.OK;
            }

            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows an MessageBox that is for information.
        /// </summary>
        /// <param name="text">The text on the messagebox.</param>
        /// <param name="caption">The title of the messagebox.</param>
        /// <param name="useNotifications">Indicates if this function should show notifications using the input notification icon.</param>
        /// <returns>A new <see cref="DialogResult"/>.</returns>
        public static DialogResult ShowInfo(string text, string caption, bool useNotifications)
        {
            if (notifyIcon != null && useNotifications)
            {
                notifyIcon.ShowBalloonTip(0, caption, text, ToolTipIcon.Info);
                return DialogResult.OK;
            }

            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows an MessageBox that is for an Warning.
        /// </summary>
        /// <param name="text">The text on the messagebox.</param>
        /// <param name="caption">The title of the messagebox.</param>
        /// <param name="useNotifications">Indicates if this function should show notifications using the input notification icon.</param>
        /// <returns>A new <see cref="DialogResult"/>.</returns>
        public static DialogResult ShowWarning(string text, string caption, bool useNotifications)
        {
            if (notifyIcon != null && useNotifications)
            {
                notifyIcon.ShowBalloonTip(0, caption, text, ToolTipIcon.Warning);
                return DialogResult.OK;
            }

            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Shows an MessageBox that is for an Question.
        /// </summary>
        /// <param name="text">The text on the messagebox.</param>
        /// <param name="caption">The title of the messagebox.</param>
        /// <returns>A new <see cref="DialogResult"/>.</returns>
        public static DialogResult ShowQuestion(string text, string caption)
            => MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MessageManager"/>
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged
        /// resources.
        /// </param>
        // holefully this is disposed of by the conatiner provided when constructing this class:
        // notifyIcon
        protected override void Dispose(bool disposing)
            => base.Dispose(disposing);
    }
}
