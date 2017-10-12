///////////////////////////////////////////////////////////////////////////////
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

using System.Diagnostics;

namespace Easemob.Common.Utils
{
	public static class DebugEx
	{
		public static void Assert(bool condition)
		{
			Debug.Assert(condition);
		}

		public static void Assert(bool condition, string message)
		{
			Debug.Assert(condition, message);
		}

		public static void Assert(bool condition, string message, string detailedMessage)
		{
			Debug.Assert(condition, message, detailedMessage);
		}

		public static void WriteLine(string text)
		{
			Debug.WriteLine(text);
		}
	}
}
