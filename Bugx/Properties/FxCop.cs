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

[module: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Bugx.Web")]
[module: SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "Bugx.Web.BugDocument")]
[module: SuppressMessage("Microsoft.Design", "CA1058:TypesShouldNotExtendCertainBaseTypes", Scope = "type", Target = "Bugx.Web.BugDocument", MessageId = "System.Xml.XmlDocument")]
[module: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "Bugx.Web.BugDocument")]
[module: SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Scope = "member", Target = "Bugx.Web.ErrorModule.IsReBug", MessageId = "Member")]
[module: SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers", Scope = "type", Target = "Bugx.Web.HttpValueCollection")]
[module: SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Scope = "member", Target = "Bugx.Web.HttpValueCollection.LoadCollectionFromXmlNode(System.Collections.Specialized.NameValueCollection,System.Xml.XmlNode):System.Void", MessageId = "System.Xml.XmlNode")]
[module: SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Scope = "member", Target = "Bugx.Web.HttpValueCollection.CreateCollectionFromUrlEncoded(System.String):Bugx.Web.HttpValueCollection", MessageId = "0#")]
[module: SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Scope = "member", Target = "Bugx.Web.HttpValueCollection.LoadFromNode(System.Xml.XmlNode):System.Void", MessageId = "System.Xml.XmlNode")]
[module: SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Scope = "member", Target = "Bugx.Web.HttpValueCollection.CreateCollectionFromXmlNode(System.Xml.XmlNode):Bugx.Web.HttpValueCollection", MessageId = "System.Xml.XmlNode")]
[module: SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Scope = "member", Target = "Bugx.Web.HttpValueCollection.SaveToNode(System.Xml.XmlNode):System.Void", MessageId = "System.Xml.XmlNode")]
[module: SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Scope = "member", Target = "Bugx.Web.HttpValueCollection.SaveCollectionToXmlNode(System.Collections.Specialized.NameValueCollection,System.Xml.XmlNode):System.Void", MessageId = "System.Xml.XmlNode")]