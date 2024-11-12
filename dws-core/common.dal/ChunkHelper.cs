﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.dal
{
	public static class ChunkHelper
	{
		public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (chunkSize <= 0)
				throw new ArgumentOutOfRangeException("chunkSize",
					"The chunkSize parameter must be a positive value.");

			return source.ChunkInternal(chunkSize);
		}

		private static IEnumerable<IEnumerable<T>> ChunkInternal<T>(this IEnumerable<T> source, int chunkSize)
		{

			// Get the enumerator.  Dispose of when done.
			using (IEnumerator<T> enumerator = source.GetEnumerator())
				do
				{
					// Move to the next element.  If there's nothing left
					// then get out.
					if (!enumerator.MoveNext()) yield break;

					// Return the chunked sequence.
					yield return ChunkSequence(enumerator, chunkSize);
				} while (true);
		}

		private static IEnumerable<T> ChunkSequence<T>(IEnumerator<T> enumerator, int chunkSize)
		{
			int count = 0;

			do
			{
				yield return enumerator.Current;
			} while (++count < chunkSize && enumerator.MoveNext());
		}
	}
}
