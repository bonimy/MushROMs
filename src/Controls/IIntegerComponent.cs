// <copyright file="IIntegerComponent.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;

namespace Controls
{
    public interface IIntegerComponent
    {
        event EventHandler ValueChanged;

        int Value { get; set; }
    }
}
