// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.ExceptionServices;
using System.Threading;
// using System.Windows.Forms;

namespace System
{
    public class ThreadExceptionFixture : IDisposable
    {
        public ThreadExceptionFixture()
        {
            //TODO: figure out how to either 
            //      1. catch thread exceptions a different way
            //      2. or import Application from System.Windows.Forms withoug having mass namespace conficts
            // Application.ThreadException += OnThreadException;
        }

        public void Dispose()
        {
            // Application.ThreadException -= OnThreadException;
        }

        private void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ExceptionDispatchInfo.Capture(e.Exception).Throw();
        }
    }
}
