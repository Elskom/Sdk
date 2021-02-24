// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.ComponentModel;
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
    public class MessageManager : Control
    {
        private readonly NotifyIcon notifyIcon;
        private readonly bool disposeIcon = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class.
        /// </summary>
        public MessageManager()
            => this.notifyIcon = new NotifyIcon();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class
        /// with the specified container.
        /// </summary>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container for the internal
        /// <see cref="NotifyIcon"/> control.
        /// </param>
        public MessageManager(IContainer container)
            : this()
        {
            this.disposeIcon = false;
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
            container.Add(this.notifyIcon);
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
                this.notifyIcon.BalloonTipClicked += value;
            }

            remove
            {
                this.notifyIcon.BalloonTipClicked -= value;
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
                this.notifyIcon.BalloonTipClosed += value;
            }

            remove
            {
                this.notifyIcon.BalloonTipClosed -= value;
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
                this.notifyIcon.BalloonTipShown += value;
            }

            remove
            {
                this.notifyIcon.BalloonTipShown -= value;
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
                this.notifyIcon.Click += value;
            }

            remove
            {
                this.notifyIcon.Click -= value;
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
                this.notifyIcon.DoubleClick += value;
            }

            remove
            {
                this.notifyIcon.DoubleClick -= value;
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
                this.notifyIcon.MouseClick += value;
            }

            remove
            {
                this.notifyIcon.MouseClick -= value;
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
                this.notifyIcon.MouseDoubleClick += value;
            }

            remove
            {
                this.notifyIcon.MouseDoubleClick -= value;
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
                this.notifyIcon.MouseDown += value;
            }

            remove
            {
                this.notifyIcon.MouseDown -= value;
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
                this.notifyIcon.MouseMove += value;
            }

            remove
            {
                this.notifyIcon.MouseMove -= value;
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
                this.notifyIcon.MouseUp += value;
            }

            remove
            {
                this.notifyIcon.MouseUp -= value;
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
            get => this.notifyIcon.BalloonTipText;

            set => this.notifyIcon.BalloonTipText = value;
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
            get => this.notifyIcon.BalloonTipIcon;

            set => this.notifyIcon.BalloonTipIcon = value;
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
            get => this.notifyIcon.BalloonTipTitle;

            set => this.notifyIcon.BalloonTipTitle = value;
        }

#if !NETCOREAPP3_1 && !NET5_0_OR_GREATER
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
            get => this.notifyIcon.ContextMenu;

            set => this.notifyIcon.ContextMenu = value;
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
            get => this.notifyIcon.ContextMenuStrip;

            set => this.notifyIcon.ContextMenuStrip = value;
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
            get => this.notifyIcon.Icon;

            set => this.notifyIcon.Icon = value;
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
            get => this.notifyIcon.Text;

            set => this.notifyIcon.Text = value;
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
            get => this.notifyIcon.Visible;

            set => this.notifyIcon.Visible = value;
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
            get => this.notifyIcon.Tag;

            set => this.notifyIcon.Tag = value;
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
        /// Shows an MessageBox that is for an Error.
        /// </summary>
        /// <param name="text">The text on the messagebox.</param>
        /// <param name="caption">The title of the messagebox.</param>
        /// <param name="useNotifications">Indicates if this function should show notifications using the input notification icon.</param>
        /// <returns>A new <see cref="DialogResult"/>.</returns>
        public DialogResult ShowError(string text, string caption, bool useNotifications)
        {
            if (this.notifyIcon != null && useNotifications)
            {
                this.notifyIcon.ShowBalloonTip(0, caption, text, ToolTipIcon.Error);
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
        public DialogResult ShowInfo(string text, string caption, bool useNotifications)
        {
            if (this.notifyIcon != null && useNotifications)
            {
                this.notifyIcon.ShowBalloonTip(0, caption, text, ToolTipIcon.Info);
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
        public DialogResult ShowWarning(string text, string caption, bool useNotifications)
        {
            if (this.notifyIcon != null && useNotifications)
            {
                this.notifyIcon.ShowBalloonTip(0, caption, text, ToolTipIcon.Warning);
                return DialogResult.OK;
            }

            return MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MessageManager"/>
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged
        /// resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.disposeIcon)
            {
                this.notifyIcon?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
