﻿///////////////////////////////////////////////////////////////////////////////
//	Copyright 2013 JASDev International
//
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at
//
//		http://www.apache.org/licenses/LICENSE-2.0
//
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Easemob.WebSocket.Client
{
	public static class WSDelegates
	{
		public delegate void ConnectionChangedEventHandler(WebSocketState websocketState);
		public delegate void TextMessageReceivedEventHandler(string textMessage);
		public delegate void DataMessageReceivedEventHandler(byte[] dataMessage);
		public delegate void ErrorEventHandler(string message, string stackTrace = null);
	}
}
