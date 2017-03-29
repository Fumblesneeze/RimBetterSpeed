using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterSpeed
{
	public static class HarmonyExtension
	{
		public static IEnumerable<T> ReplaceMatchingSequence<T>(this IEnumerable<T> source, IEnumerable<T> elements, Func<IEnumerable<T>, IEnumerable<T>> replacer)
		{
			return ReplaceMatchingSequence(source, elements.Select(x => (Func<T, bool>)(y => y.Equals(x))), replacer);
		}

		public static IEnumerable<T> ReplaceMatchingSequence<T>(this IEnumerable<T> source, IEnumerable<Func<T, bool>> predicates, Func<IEnumerable<T>, IEnumerable<T>> replacer)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (predicates == null)
				throw new ArgumentNullException(nameof(predicates));

			var p = predicates.ToArray();

			if (p.Length == 0)
				throw new ArgumentException("Count of predicates is zero.");

			if (replacer == null)
				throw new ArgumentNullException(nameof(replacer));

			return ReplaceMatchingSequenceInner(source, predicates, replacer);
		}

		private static IEnumerable<T> ReplaceMatchingSequenceInner<T>(this IEnumerable<T> source, IEnumerable<Func<T, bool>> predicates, Func<IEnumerable<T>, IEnumerable<T>> replacer)
		{
			var p = predicates.ToArray();
			var matchingIndex = -1;

			var buffer = new List<T>();

			foreach (var e in source)
			{
				// matched next predicate
				if (p[matchingIndex + 1](e))
				{
					matchingIndex++;

					// add item to buffer
					buffer.Add(e);

					// all predicates matched
					if (matchingIndex + 1 == p.Length)
					{
						matchingIndex = -1;
						foreach (var r in replacer(buffer))
						{
							yield return r;
						}
						buffer.Clear();
					}
				}
				else
				{
					// no match, so clear buffer if any and reset index
					foreach (var i in buffer)
						yield return i;
					buffer.Clear();
					matchingIndex = -1;

					// then return current item
					yield return e;
				}
			}

			// finished going through source, send out the buffer if any
			foreach (var l in buffer)
				yield return l;
		}
	}
}
