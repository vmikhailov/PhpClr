﻿using System;
using System.Collections.Generic;

namespace PhpClr.Parsers.PhpParser.Toolbox
{
    public sealed class Disposable : IDisposable
    {
        public Disposable()
        {
        }

        public Disposable(params Action[] actions)
        {
            DisposeActions.AddRange(actions);
        }

        private List<Action> DisposeActions { get; } = new List<Action>();

        public void Dispose()
        {
            var actions = DisposeActions.ToArray();
            for (var i = actions.Length - 1; i >= 0; i--)
            {
                var action = actions[i];
                DisposeActions.RemoveAt(i);
                action?.Invoke();
            }
        }
    }
}
