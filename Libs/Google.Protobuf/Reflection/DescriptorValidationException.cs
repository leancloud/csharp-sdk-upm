#region Copyright notice and license
// Protocol Buffers - Google's data interchange format
// Copyright 2008 Google Inc.  All rights reserved.
// https://developers.google.com/protocol-buffers/
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of Google Inc. nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;

namespace LC.Google.Protobuf.Reflection
{
    /// <summary>
    /// Thrown when building descriptors fails because the source DescriptorProtos
    /// are not valid.
    /// </summary>
    public sealed class DescriptorValidationException : Exception
    {
        private readonly String name;
        private readonly string description;

        /// <value>
        /// The full name of the descriptor where the error occurred.
        /// </value>
        public String ProblemSymbolName
        {
            get { return name; }
        }

        /// <value>
        /// A human-readable description of the error. (The Message property
        /// is made up of the descriptor's name and this description.)
        /// </value>
        public string Description
        {
            get { return description; }
        }

        internal DescriptorValidationException(IDescriptor problemDescriptor, string description) :
            base(problemDescriptor.FullName + ": " + description)
        {
            // Note that problemDescriptor may be partially uninitialized, so we
            // don't want to expose it directly to the user.  So, we only provide
            // the name and the original proto.
            name = problemDescriptor.FullName;
            this.description = description;
        }

        internal DescriptorValidationException(IDescriptor problemDescriptor, string description, Exception cause) :
            base(problemDescriptor.FullName + ": " + description, cause)
        {
            name = problemDescriptor.FullName;
            this.description = description;
        }
    }
}