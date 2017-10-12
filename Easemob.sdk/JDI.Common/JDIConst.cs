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


namespace Easemob.Common
{
	public static class JDIConst
	{
		public enum ByteOrder
		{
			Network = 0,
			BigEndian = 1,
			LittleEndian = 2
		}

		public const int MicrosecondsPerSecond = 1000000;
		public const int MillisecondsPerSecond = 1000;
	}
}