// Copyright (c) 2018-2021, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs;

/// <summary>Represents the method that will handle an event when the event provides data.</summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">An object that contains the event data.</param>
public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
