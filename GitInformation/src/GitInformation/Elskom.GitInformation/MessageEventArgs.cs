// Copyright (c) 2019-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;

    /// <summary>
    /// Event that holds the message text and the caption.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs"/> class.
        /// </summary>
        /// <param name="text">The text for the message.</param>
        /// <param name="caption">The title (caption) for the message.</param>
        /// <param name="errorlevel">The error level for the message, or <see cref="ErrorLevel.None"/> for no error level information.</param>
        public MessageEventArgs(string text, string caption, ErrorLevel errorlevel)
        {
            this.Text = text;
            this.Caption = caption;
            this.ErrorLevel = errorlevel;
        }

        /// <summary>
        /// Gets or sets the text for the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the caption (title) for the message.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Libs.ErrorLevel"/> of the message.
        /// </summary>
        public ErrorLevel ErrorLevel { get; set; }
    }
}
