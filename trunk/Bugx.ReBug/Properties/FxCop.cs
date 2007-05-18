/*
BUGx: An Asp.Net Bug Tracking tool.
Copyright (C) 2007 Olivier Bossaer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Wavenet, hereby disclaims all copyright interest in
the library `BUGx' (An Asp.Net Bug Tracking tool) written
by Olivier Bossaer. (olivier.bossaer@gmail.com)
*/

using System.Diagnostics.CodeAnalysis;


[module: SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Scope = "namespace", Target = "Bugx.ReBug")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Bugx.ReBug.BugxWorkerRequest..cctor()")]
[module: SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Scope = "member", Target = "Bugx.ReBug.BugxWorkerRequest..ctor(Bugx.ReBug.ReBugContext,System.IO.TextWriter)")]
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "Bugx.ReBug.BugxWorkerRequest.DefineHeader(System.Int32,System.String,System.String):System.Void", MessageId = "serverVariable")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Bugx.ReBug.BugxHost.Process(Bugx.ReBug.ReBugContext):System.Void")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Bugx.ReBug.BugxHost.CheckAppDomainConnection():System.Void")]
[module: SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Scope = "type", Target = "Bugx.ReBug.ReBugContext")]